package controllers

import (
	commands "product-service/internal/application/commands/products"
	"product-service/internal/application/mediator"
	queries "product-service/internal/application/queries/products"
	"product-service/internal/models/products"

	"github.com/gofiber/fiber/v2"
)

type ProductController struct {
	dispatcher *mediator.Dispatcher
}

func NewProductController(dispatcher *mediator.Dispatcher) *ProductController {
	return &ProductController{dispatcher: dispatcher}
}

func (c *ProductController) GetProductByID(ctx *fiber.Ctx) error {
	query := &queries.GetProductQueryById{
		ID: ctx.Params("id"),
	}

	res, err := mediator.Send[*queries.GetProductQueryById, *queries.GetProductQueryByIdResponse](c.dispatcher, ctx.Context(), query)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{
			"error": err.Error(),
		})
	}

	return ctx.JSON(res)
}

func (c *ProductController) CreateProduct(ctx *fiber.Ctx) error {
	var req products.CreateProductRequest
	if err := ctx.BodyParser(&req); err != nil {
		return fiber.NewError(fiber.StatusBadRequest, "invalid request body")
	}

	res, err := mediator.Send[*commands.CreateProductCommand, *commands.CreateProductCommandResponse](c.dispatcher, ctx.Context(), req.ToCommand())
	if err != nil {
		return err
	}

	return ctx.Status(fiber.StatusCreated).JSON(res)
}

func (c *ProductController) RegisterRoutes(app *fiber.App) {
	app.Get("api/product/:id", c.GetProductByID)
	app.Post("api/product", c.CreateProduct)
}

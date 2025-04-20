package controllers

import (
	"product-service/internal/application/mediator"
	queries "product-service/internal/application/queries/products"

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

func (c *ProductController) RegisterRoutes(app *fiber.App) {
	app.Get("api/products/:id", c.GetProductByID)
}

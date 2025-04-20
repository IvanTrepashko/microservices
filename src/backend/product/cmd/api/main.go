package main

import (
	"product-service/internal/application/mediator"
	queries "product-service/internal/application/queries/products"
	"product-service/internal/controllers"

	"github.com/gofiber/fiber/v2"
)

func main() {
	app := fiber.New(fiber.Config{
		ErrorHandler: customErrorHandler,
	})

	// Init core dependencies
	dispatcher := mediator.NewDispatcher()

	// Register all query/command handlers
	registerHandlers(dispatcher)

	// Register all controllers
	registerControllers(app, dispatcher)

	app.Listen(":3000")
}

func registerHandlers(dispatcher *mediator.Dispatcher) {
	// Query handlers
	mediator.Register(
		dispatcher,
		queries.NewGetProductQueryByIdHandler(),
	)
}

func registerControllers(app *fiber.App, dispatcher *mediator.Dispatcher) {
	// Product routes
	productController := controllers.NewProductController(dispatcher)
	productController.RegisterRoutes(app)
}

func customErrorHandler(ctx *fiber.Ctx, err error) error {
	// Handle specific errors and return appropriate status codes
	return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{
		"error": err.Error(),
	})
}

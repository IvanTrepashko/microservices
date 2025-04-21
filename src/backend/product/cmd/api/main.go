package main

import (
	"log"
	commands "product-service/internal/application/commands/products"
	"product-service/internal/application/mediator"
	queries "product-service/internal/application/queries/products"
	"product-service/internal/controllers"
	"product-service/internal/infrastructure/database"
	"product-service/internal/infrastructure/repository"

	"github.com/gofiber/fiber/v2"
)

func main() {
	app := fiber.New(fiber.Config{
		ErrorHandler: customErrorHandler,
	})

	db, err := database.NewMongoDatabase()
	if err != nil {
		log.Fatal(err)
	}

	productRepo := repository.NewProductRepository(db)

	// Init core dependencies
	dispatcher := mediator.NewDispatcher()

	// Register all query/command handlers
	registerHandlers(dispatcher, productRepo)

	// Register all controllers
	registerControllers(app, dispatcher)

	app.Listen(":3000")
}

func registerHandlers(dispatcher *mediator.Dispatcher, productRepo *repository.ProductRepository) {
	// Query handlers
	mediator.Register(
		dispatcher,
		queries.NewGetProductQueryByIdHandler(productRepo),
	)

	// Command handlers
	mediator.Register(
		dispatcher,
		commands.NewCreateProductCommandHandler(productRepo),
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

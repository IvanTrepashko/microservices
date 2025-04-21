package commands

import (
	"context"
	"product-service/internal/domain/entities"
	"product-service/internal/domain/repository"
)

type CreateProductCommand struct {
	Name       string
	Price      float64
	Stock      int64
	CategoryID string
}

type CreateProductCommandHandler struct {
	productRepository repository.ProductRepository
}

type CreateProductCommandResponse struct {
	ID string
}

func NewCreateProductCommandHandler(productRepository repository.ProductRepository) *CreateProductCommandHandler {
	return &CreateProductCommandHandler{productRepository: productRepository}
}

func (h *CreateProductCommandHandler) Handle(ctx context.Context, command *CreateProductCommand) (*CreateProductCommandResponse, error) {
	product := entities.NewProduct(command.Name, command.Price, command.Stock, command.CategoryID)
	err := h.productRepository.CreateProduct(ctx, product)
	if err != nil {
		return nil, err
	}
	return &CreateProductCommandResponse{ID: product.ID}, nil
}

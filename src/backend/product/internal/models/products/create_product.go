package products

import (
	commands "product-service/internal/application/commands/products"
)

type CreateProductRequest struct {
	Name       string
	Price      float64
	Stock      int64
	CategoryID string
}

type CreateProductResponse struct {
	ID string
}

func (r *CreateProductRequest) ToCommand() *commands.CreateProductCommand {
	return &commands.CreateProductCommand{
		Name:       r.Name,
		Price:      r.Price,
		Stock:      r.Stock,
		CategoryID: r.CategoryID,
	}
}

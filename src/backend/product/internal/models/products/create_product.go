package products

import (
	commands "product-service/internal/application/commands/products"
)

// CreateProductRequest represents product creation payload
type CreateProductRequest struct {
	Name       string  `json:"name" example:"iPhone 13"`
	Price      float64 `json:"price" example:"999.99"`
	Stock      int64   `json:"stock" example:"100"`
	CategoryID string  `json:"category_id" example:"phones"`
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

package queries

import (
	"context"
	"errors"
	"product-service/internal/domain/repository"
)

type GetProductQueryById struct {
	ID string
}

type GetProductQueryByIdResponse struct {
	ID    string  `json:"id"`
	Name  string  `json:"name"`
	Price float64 `json:"price"`
	Stock int64   `json:"stock"`
}

type GetProductQueryByIdHandler struct {
	productRepository repository.ProductRepository
}

func NewGetProductQueryByIdHandler(productRepository repository.ProductRepository) *GetProductQueryByIdHandler {
	return &GetProductQueryByIdHandler{
		productRepository: productRepository,
	}
}

func (h *GetProductQueryByIdHandler) Handle(ctx context.Context, req *GetProductQueryById) (*GetProductQueryByIdResponse, error) {
	res, err := h.productRepository.GetProductByID(ctx, req.ID)
	if err != nil {
		return nil, err
	}

	if res == nil {
		return nil, errors.New("product not found")
	}

	return &GetProductQueryByIdResponse{
		ID:    res.ID,
		Name:  res.Name,
		Price: res.Price,
		Stock: res.Stock,
	}, nil
}

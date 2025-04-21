package repository

import (
	"context"
	"product-service/internal/domain/entities"
)

type ProductRepository interface {
	GetProductByID(ctx context.Context, id string) (*entities.Product, error)
	CreateProduct(ctx context.Context, product *entities.Product) error
}

package repository

import (
	"context"
	"errors"
	"product-service/internal/domain/entities"

	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
)

type ProductRepository struct {
	collection *mongo.Collection
}

func NewProductRepository(db *mongo.Database) *ProductRepository {
	return &ProductRepository{
		collection: db.Collection("products"),
	}
}

func (r *ProductRepository) GetProductByID(ctx context.Context, id string) (*entities.Product, error) {
	var product entities.Product
	err := r.collection.FindOne(ctx, bson.M{"_id": id}).Decode(&product)
	if err != nil {
		if err == mongo.ErrNoDocuments {
			return nil, errors.New("product not found")
		}
		return nil, err
	}
	return &product, nil
}

func (r *ProductRepository) CreateProduct(ctx context.Context, product *entities.Product) error {
	_, err := r.collection.InsertOne(ctx, product)
	if err != nil {
		return err
	}
	return nil
}

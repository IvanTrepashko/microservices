package entities

import (
	"time"

	"github.com/google/uuid"
)

type Product struct {
	ID         string    `bson:"_id" json:"id"` // UUID stored as string
	Name       string    `bson:"name" json:"name"`
	Price      float64   `bson:"price" json:"price"`
	Stock      int64     `bson:"stock" json:"stock"`
	CategoryID string    `bson:"category_id" json:"category_id"`
	CreatedAt  time.Time `bson:"created_at" json:"created_at"`
	UpdatedAt  time.Time `bson:"updated_at" json:"updated_at"`
	Revision   int64     `bson:"revision" json:"revision"`
}

func NewProduct(name string, price float64, stock int64, categoryID string) *Product {
	return &Product{
		ID:         uuid.New().String(),
		Name:       name,
		Price:      price,
		Stock:      stock,
		CategoryID: categoryID,
		CreatedAt:  time.Now(),
		UpdatedAt:  time.Now(),
		Revision:   1,
	}
}

func (p *Product) Update(name string, price float64, stock int64) {
	p.Name = name
	p.Price = price
	p.Stock = stock
	p.UpdatedAt = time.Now()
	p.Revision++
}

func (p *Product) UpdateCategory(categoryID string) {
	p.CategoryID = categoryID
	p.UpdatedAt = time.Now()
	p.Revision++
}

func (p *Product) UpdatePrice(price float64) {
	p.Price = price
	p.UpdatedAt = time.Now()
	p.Revision++
}

func (p *Product) DecreaseStock(quantity int64) {
	p.Stock -= quantity
	p.UpdatedAt = time.Now()
	p.Revision++
}

func (p *Product) IncreaseStock(quantity int64) {
	p.Stock += quantity
	p.UpdatedAt = time.Now()
	p.Revision++
}

func (p *Product) IsAvailable() bool {
	return p.Stock > 0
}

func (p *Product) IsOutOfStock() bool {
	return p.Stock <= 0
}

package queries

import "context"

type GetProductQueryById struct {
	ID string
}

type GetProductQueryByIdResponse struct {
	ID   string `json:"id"`
	Name string `json:"name"`
}

type GetProductQueryByIdHandler struct {
}

func NewGetProductQueryByIdHandler() *GetProductQueryByIdHandler {
	return &GetProductQueryByIdHandler{}
}

func (h *GetProductQueryByIdHandler) Handle(ctx context.Context, req *GetProductQueryById) (*GetProductQueryByIdResponse, error) {
	return &GetProductQueryByIdResponse{
		ID:   req.ID,
		Name: "Product " + req.ID,
	}, nil
}

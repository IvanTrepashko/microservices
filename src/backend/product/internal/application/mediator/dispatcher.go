package mediator

import (
	"context"
	"errors"
	"reflect"
)

type Request any
type Response any

type RequestHandler[TReq Request, TRes Response] interface {
	Handle(ctx context.Context, req TReq) (TRes, error)
}

type Dispatcher struct {
	handlers map[string]any
}

func NewDispatcher() *Dispatcher {
	return &Dispatcher{
		handlers: make(map[string]any),
	}
}

func Register[TReq Request, TRes Response](d *Dispatcher, handler RequestHandler[TReq, TRes]) {
	d.handlers[getTypeName[TReq]()] = handler
}

func Send[TReq Request, TRes Response](d *Dispatcher, ctx context.Context, req TReq) (TRes, error) {
	handlerKey := getTypeName[TReq]()
	if handler, ok := d.handlers[handlerKey].(RequestHandler[TReq, TRes]); ok {
		return handler.Handle(ctx, req)
	}
	var zero TRes
	return zero, errors.New("handler not found")
}

func getTypeName[T any]() string {
	return reflect.TypeOf((*T)(nil)).Elem().String()
}

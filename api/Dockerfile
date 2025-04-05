# Stage 1: Build the Go app
FROM golang:1.24.1-alpine AS builder

WORKDIR /api

COPY go.mod go.sum ./
RUN go mod download

COPY . .

ENV CGO_ENABLED=0 GOOS=linux GOARCH=amd64
RUN go build -o main .

# Stage 2: Lightweight runtime image
FROM alpine:latest

WORKDIR /api

COPY --from=builder /api/main .

RUN chmod +x main

EXPOSE 8080

CMD ["./main"]

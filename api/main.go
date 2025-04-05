package main

import (
	"log"

	"perlin/server"
)

func main() {
	r := server.SetupRouter()
	log.Println("Server running on :8080")
	r.Run(":8080")
}

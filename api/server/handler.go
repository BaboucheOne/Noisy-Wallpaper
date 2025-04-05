package server

import (
	"bytes"
	"encoding/json"
	"net/http"

	"perlin/generator"
	"perlin/models"

	"github.com/gin-gonic/gin"
)

func handleImage(c *gin.Context) {
	var params models.Params
	if err := json.NewDecoder(c.Request.Body).Decode(&params); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": "Invalid JSON"})
		return
	}

	grad := generator.GenerateGradient(params.Colors)
	img := generator.GenerateNoise(params, grad)

	buf := new(bytes.Buffer)
	if err := generator.EncodePNG(img, buf); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": "Failed to encode image"})
		return
	}

	c.Header("Content-Type", "image/png")
	c.Writer.Write(buf.Bytes())
}

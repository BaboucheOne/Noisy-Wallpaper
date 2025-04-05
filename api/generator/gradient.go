package generator

import (
	"fmt"

	"github.com/mazznoer/colorgrad"
)

func GenerateGradient(colors []string) colorgrad.Gradient {
	grad, err := colorgrad.NewGradient().HtmlColors(colors...).Build()
	if err != nil {
		panic(fmt.Sprintf("Failed to generate gradient: %v", err))
	}
	return grad
}

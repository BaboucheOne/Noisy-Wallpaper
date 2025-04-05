package generator

import (
	"image"
	"image/png"
	"io"
)

func EncodePNG(img *image.RGBA, w io.Writer) error {
	return png.Encode(w, img)
}

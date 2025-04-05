package generator

import (
	"image"
	"image/color"
	"math"
	"math/rand"
	"sync"

	"perlin/models"

	"github.com/aquilax/go-perlin"
	"github.com/mazznoer/colorgrad"
)

func GenerateNoise(params models.Params, grad colorgrad.Gradient) *image.RGBA {
	img := image.NewRGBA(image.Rect(0, 0, params.Width, params.Height))
	p := perlin.NewPerlin(2, 2, 3, rand.Int63())

	noiseMap := createNoiseMap(params, p)
	voronoiMap := GenerateVoronoi(params)

	wg := sync.WaitGroup{}
	wg.Add(params.Height)

	for y := 0; y < params.Height; y++ {
		go func(y int) {
			defer wg.Done()
			for x := 0; x < params.Width; x++ {
				blend := (1-params.BlendFactor)*noiseMap[y][x] + params.BlendFactor*voronoiMap[y][x]
				c := grad.At(blend)
				r, g, b, _ := c.RGBA()
				img.Set(x, y, color.RGBA{uint8(r >> 8), uint8(g >> 8), uint8(b >> 8), 255})
			}
		}(y)
	}
	wg.Wait()

	return img
}

func createNoiseMap(params models.Params, p *perlin.Perlin) [][]float64 {
	noiseMap := make([][]float64, params.Height)
	wg := sync.WaitGroup{}
	wg.Add(params.Height)

	for y := 0; y < params.Height; y++ {
		noiseMap[y] = make([]float64, params.Width)
		go func(y int) {
			defer wg.Done()
			for x := 0; x < params.Width; x++ {
				freq, amp := params.Frequency, params.Amplitude
				noiseVal := 0.0
				for i := 0; i < params.Octaves; i++ {
					noiseVal += p.Noise2D(float64(x)/params.Scale*freq, float64(y)/params.Scale*freq) * amp
					freq *= 2
					amp /= 2
				}
				noiseMap[y][x] = noiseVal
			}
		}(y)
	}
	wg.Wait()

	normalizeMap(noiseMap)
	return noiseMap
}

func normalizeMap(noiseMap [][]float64) {
	minVal, maxVal := math.Inf(1), math.Inf(-1)
	for y := range noiseMap {
		for x := range noiseMap[y] {
			if noiseMap[y][x] < minVal {
				minVal = noiseMap[y][x]
			}
			if noiseMap[y][x] > maxVal {
				maxVal = noiseMap[y][x]
			}
		}
	}
	for y := range noiseMap {
		for x := range noiseMap[y] {
			noiseMap[y][x] = (noiseMap[y][x] - minVal) / (maxVal - minVal)
		}
	}
}

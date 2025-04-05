package generator

import (
	"math"
	"math/rand"
	"sync"

	"perlin/models"
)

func GenerateVoronoi(params models.Params) [][]float64 {
	voronoiMap := make([][]float64, params.Height)
	points := make([][2]int, params.NumPoints)

	for i := 0; i < params.NumPoints; i++ {
		points[i] = [2]int{rand.Intn(params.Width), rand.Intn(params.Height)}
	}

	wg := sync.WaitGroup{}
	wg.Add(params.Height)

	for y := 0; y < params.Height; y++ {
		voronoiMap[y] = make([]float64, params.Width)
		go func(y int) {
			defer wg.Done()
			for x := 0; x < params.Width; x++ {
				minDist := math.Inf(1)
				for _, pt := range points {
					dist := math.Hypot(float64(pt[0]-x), float64(pt[1]-y))
					if dist < minDist {
						minDist = dist
					}
				}
				voronoiMap[y][x] = minDist
			}
		}(y)
	}
	wg.Wait()

	normalizeMap(voronoiMap)
	return voronoiMap
}

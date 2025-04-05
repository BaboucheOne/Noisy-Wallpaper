package models

type Params struct {
	Width       int      `json:"Width"`
	Height      int      `json:"Height"`
	Scale       float64  `json:"Scale"`
	Octaves     int      `json:"Octaves"`
	Frequency   float64  `json:"Frequency"`
	Amplitude   float64  `json:"Amplitude"`
	BlendFactor float64  `json:"BlendFactor"`
	NumPoints   int      `json:"NumberOfPoints"`
	Colors      []string `json:"Colors"`
}

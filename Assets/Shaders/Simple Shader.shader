Shader "Rustic/SimpleShader" {
	Properties {
		_Color("Main Color", Color) = (1, 1, 1)
	}
	SubShader {
		Pass {
			Material {
				Diffuse [_Color]
			}
			Lighting On
		}
	}
}

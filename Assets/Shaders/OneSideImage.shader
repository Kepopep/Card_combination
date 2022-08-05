Shader "GUI/OneSideImage"
{
	Properties{
		_MainTex("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		
		Lighting Off Cull Back ZWrite Off Fog{ Mode Off }

		Blend SrcAlpha OneMinusSrcAlpha
	
		Pass
		{
			SetTexture[_MainTex]
		}
	}
}

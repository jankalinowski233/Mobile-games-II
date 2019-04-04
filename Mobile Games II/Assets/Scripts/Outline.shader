Shader "Unlit/Outline"
{
	//shader properties
    Properties
    {
		_Color("Main Color", Color) = (0.5, 0.5, 0.5, 1)
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor("Outline color", Color) = (1,0,0,1)
		_OutlineWidth("Outline width", Range(0.0, 5.0)) = 0.0
    }
		CGINCLUDE
		#include "UnityCG.cginc"

			//structs - vertices positions and normals
		struct appdata
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		//structs - positions, colors and normals
		struct v2f
		{
			float4 pos : POSITION;
			float4 color : COLOR;
			float3 normal : NORMAL;
		};

		//outline properties
		float _OutlineWidth;
		float4 _OutlineColor;		//float4 - RGBA

		v2f vert(appdata v)
		{
			v.vertex.xyz *= _OutlineWidth;

			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			return o;
		}

		ENDCG

    SubShader
    {
        Pass 
		{
			ZWrite Off //disable zWrite - z Axis

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

			//set outline colour
			half4 frag(v2f i) : COLOR
			{
				return _OutlineColor;
			}
				
			ENDCG
		}

		Pass
		{
				ZWrite On //enable zWrite - z Axis

				//get material info
				Material
				{
					Diffuse[_Color]
					Ambient[_Color]
				}

				Lighting On

				//set texture
				SetTexture[_MainTex]
				{
					ConstantColor[_Color]
				}

				//overwrite texture
				SetTexture[_MainTex]
				{
					Combine previous * primary DOUBLE
				}
		}
        
    }
}

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Crystal" {
	Properties {
		_CrystalColor ("Crystal Color", COLOR) = (1, 1, 1, 1)
		_SolidColor("Solid Color", COLOR) = (0, 0, 0, 1)
		_Minimum ("Minimum", RANGE(0, 1)) = 1
		_Falloff ("Falloff", FLOAT) = 20
		_Zoom ("Perlin Zoom", FLOAT) = 10
		[Toggle] _UseDiffuse ("Use Diffuse", INT) = 0
		_Distance ("Light Distance", RANGE(1, 50)) = 1
		_Kc ("Light Constant", RANGE(0.1, 5)) = 1
		_Kl("Light Linear", RANGE(0, 5)) = 1
		_Kq("Light Quadratic", RANGE(0, 5)) = 1
	}
	SubShader {
		Pass {
			Tags {
				"RenderType" = "TransparentCutout"
				"LightMode" = "ForwardBase"
			}
			Cull Off
			LOD 200

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag


			float4 permute(float4 x) { return fmod(((x*34.0) + 1.0)*x, 289.0); }
			float4 taylorInvSqrt(float4 r) { return 1.79284291400159 - 0.85373472095314 * r; }
			float3 fade(float3 t) { return t*t*t*(t*(t*6.0 - 15.0) + 10.0); }
			float cnoise(float3 P)
			{
				float3 Pi0 = floor(P); // Integer part for indexing
				float3 Pi1 = Pi0 + float3(1, 1, 1); // Integer part + 1
				Pi0 = fmod(Pi0, 289.0);
				Pi1 = fmod(Pi1, 289.0);
				float3 Pf0 = frac(P); // fracional part for interpolation
				float3 Pf1 = Pf0 - float3(1, 1, 1); // fracional part - 1.0
				float4 ix = float4(Pi0.x, Pi1.x, Pi0.x, Pi1.x);
				float4 iy = float4(Pi0.yy, Pi1.yy);
				float4 iz0 = Pi0.zzzz;
				float4 iz1 = Pi1.zzzz;

				float4 ixy = permute(permute(ix) + iy);
				float4 ixy0 = permute(ixy + iz0);
				float4 ixy1 = permute(ixy + iz1);

				float4 gx0 = ixy0 / 7.0;
				float4 gy0 = frac(floor(gx0) / 7.0) - 0.5;
				gx0 = frac(gx0);
				float4 gz0 = float4(0.5, 0.5, 0.5, 0.5) - abs(gx0) - abs(gy0);
				float4 sz0 = step(gz0, float4(0, 0, 0, 0));
				gx0 -= sz0 * (step(0.0, gx0) - 0.5);
				gy0 -= sz0 * (step(0.0, gy0) - 0.5);

				float4 gx1 = ixy1 / 7.0;
				float4 gy1 = frac(floor(gx1) / 7.0) - 0.5;
				gx1 = frac(gx1);
				float4 gz1 = float4(0.5, 0.5, 0.5, 0.5) - abs(gx1) - abs(gy1);
				float4 sz1 = step(gz1, float4(0, 0, 0, 0));
				gx1 -= sz1 * (step(0.0, gx1) - 0.5);
				gy1 -= sz1 * (step(0.0, gy1) - 0.5);

				float3 g000 = float3(gx0.x, gy0.x, gz0.x);
				float3 g100 = float3(gx0.y, gy0.y, gz0.y);
				float3 g010 = float3(gx0.z, gy0.z, gz0.z);
				float3 g110 = float3(gx0.w, gy0.w, gz0.w);
				float3 g001 = float3(gx1.x, gy1.x, gz1.x);
				float3 g101 = float3(gx1.y, gy1.y, gz1.y);
				float3 g011 = float3(gx1.z, gy1.z, gz1.z);
				float3 g111 = float3(gx1.w, gy1.w, gz1.w);

				float4 norm0 = taylorInvSqrt(float4(dot(g000, g000), dot(g010, g010), dot(g100, g100), dot(g110, g110)));
				g000 *= norm0.x;
				g010 *= norm0.y;
				g100 *= norm0.z;
				g110 *= norm0.w;
				float4 norm1 = taylorInvSqrt(float4(dot(g001, g001), dot(g011, g011), dot(g101, g101), dot(g111, g111)));
				g001 *= norm1.x;
				g011 *= norm1.y;
				g101 *= norm1.z;
				g111 *= norm1.w;

				float n000 = dot(g000, Pf0);
				float n100 = dot(g100, float3(Pf1.x, Pf0.yz));
				float n010 = dot(g010, float3(Pf0.x, Pf1.y, Pf0.z));
				float n110 = dot(g110, float3(Pf1.xy, Pf0.z));
				float n001 = dot(g001, float3(Pf0.xy, Pf1.z));
				float n101 = dot(g101, float3(Pf1.x, Pf0.y, Pf1.z));
				float n011 = dot(g011, float3(Pf0.x, Pf1.yz));
				float n111 = dot(g111, Pf1);

				float3 fade_xyz = fade(Pf0);
				float4 n_z = lerp(float4(n000, n100, n010, n110), float4(n001, n101, n011, n111), fade_xyz.z);
				float2 n_yz = lerp(n_z.xy, n_z.zw, fade_xyz.y);
				float n_xyz = lerp(n_yz.x, n_yz.y, fade_xyz.x);
				return 1.1 * n_xyz + 0.5;
			}

			struct vIn
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct vOut
			{
				float4 position : SV_POSITION;
				float4 texcoord : TEXCOORD0;
				float4 worldpos : TEXCOORD1;
				float4 diffusal : COLOR;
			};

			float4 _CrystalColor;
			float4 _SolidColor;
			float _Minimum;
			float _Falloff;
			float _Zoom;
			bool _UseDiffuse;
			float _Distance, _Kc, _Kl, _Kq;

			uniform float4 _LightColor0;

			vOut vert(vIn input)
			{
				vOut output;

				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				float3 normalDirection = normalize(
					mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				float3 lightDirection;
				float attenuation;

				if (0.0 == _WorldSpaceLightPos0.w) // Directional
				{
					attenuation = 1.0;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				}
				else // Other
				{
					float3 vertexToLightSource = _WorldSpaceLightPos0.xyz
						- mul(modelMatrix, input.vertex).xyz;
					float distance = length(vertexToLightSource);
					distance = distance / _Distance;
					attenuation = _Distance / (_Kc + distance * _Kl + distance * distance * _Kq);
					lightDirection = normalize(vertexToLightSource);
				}

				float3 diffuseReflection =
					attenuation * _LightColor0.rgb * max(0.0, dot(normalDirection, lightDirection));

				output.diffusal = float4(diffuseReflection, 1.0);
				output.texcoord = input.texcoord;
				output.position = UnityObjectToClipPos(input.vertex);
				output.worldpos = mul(unity_ObjectToWorld, input.vertex);
				return output;
			}

			float4 frag(vOut input) : COLOR
			{
				bool useDiffuse = _UseDiffuse;
				float4 color;
				float4 crystal = _CrystalColor;
				float4 solid = _SolidColor;
				float minimum = _Minimum;
				float falloff = _Falloff;
				float zoom = _Zoom;

				const float offset = 0.05;
				float perlin = 0.0;
/*
				for (int X = -1; X <= 1; X++)
				{
					for (int Y = -1; Y <= 1; Y++)
					{
						for (int Z = -1; Z <= 1; Z++)
						{
							int where = abs(X) + abs(Y) + abs(Z);
							int amount;
							if (where == 3)
								amount = -1;
							else if (where == 2)
								amount = -2;
							else if (where == 1)
								amount = -4;
							else
								amount = 8 * 1 + 12 * 2 + 6 * 4 + 1;
							float3 p = input.worldpos.xyz * zoom;
							p.x = p.x + X * offset;
							p.y = p.y + Y * offset;
							p.z = p.z + Z * offset;
							perlin += cnoise(p) * amount;
						}
					}
				}
*/
				perlin = cnoise(input.worldpos.xyz * zoom);

				float fall = 1 + abs(perlin - minimum) * (falloff);

				if (perlin > minimum)
					color = (1 - perlin) * (2 / fall) * crystal;
				else
					color = solid;
				if (useDiffuse)
					return color * input.diffusal;
				else
					return color;
			}

			ENDCG
		}
		Pass{
			Tags{
			"RenderType" = "TransparentCutout"
			"LightMode" = "ForwardAdd"
		}
			Cull Off
			LOD 200

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag


			float4 permute(float4 x) { return fmod(((x*34.0) + 1.0)*x, 289.0); }
			float4 taylorInvSqrt(float4 r) { return 1.79284291400159 - 0.85373472095314 * r; }
			float3 fade(float3 t) { return t*t*t*(t*(t*6.0 - 15.0) + 10.0); }
			float cnoise(float3 P)
			{
				float3 Pi0 = floor(P); // Integer part for indexing
				float3 Pi1 = Pi0 + float3(1, 1, 1); // Integer part + 1
				Pi0 = fmod(Pi0, 289.0);
				Pi1 = fmod(Pi1, 289.0);
				float3 Pf0 = frac(P); // fracional part for interpolation
				float3 Pf1 = Pf0 - float3(1, 1, 1); // fracional part - 1.0
				float4 ix = float4(Pi0.x, Pi1.x, Pi0.x, Pi1.x);
				float4 iy = float4(Pi0.yy, Pi1.yy);
				float4 iz0 = Pi0.zzzz;
				float4 iz1 = Pi1.zzzz;

				float4 ixy = permute(permute(ix) + iy);
				float4 ixy0 = permute(ixy + iz0);
				float4 ixy1 = permute(ixy + iz1);

				float4 gx0 = ixy0 / 7.0;
				float4 gy0 = frac(floor(gx0) / 7.0) - 0.5;
				gx0 = frac(gx0);
				float4 gz0 = float4(0.5, 0.5, 0.5, 0.5) - abs(gx0) - abs(gy0);
				float4 sz0 = step(gz0, float4(0, 0, 0, 0));
				gx0 -= sz0 * (step(0.0, gx0) - 0.5);
				gy0 -= sz0 * (step(0.0, gy0) - 0.5);

				float4 gx1 = ixy1 / 7.0;
				float4 gy1 = frac(floor(gx1) / 7.0) - 0.5;
				gx1 = frac(gx1);
				float4 gz1 = float4(0.5, 0.5, 0.5, 0.5) - abs(gx1) - abs(gy1);
				float4 sz1 = step(gz1, float4(0, 0, 0, 0));
				gx1 -= sz1 * (step(0.0, gx1) - 0.5);
				gy1 -= sz1 * (step(0.0, gy1) - 0.5);

				float3 g000 = float3(gx0.x, gy0.x, gz0.x);
				float3 g100 = float3(gx0.y, gy0.y, gz0.y);
				float3 g010 = float3(gx0.z, gy0.z, gz0.z);
				float3 g110 = float3(gx0.w, gy0.w, gz0.w);
				float3 g001 = float3(gx1.x, gy1.x, gz1.x);
				float3 g101 = float3(gx1.y, gy1.y, gz1.y);
				float3 g011 = float3(gx1.z, gy1.z, gz1.z);
				float3 g111 = float3(gx1.w, gy1.w, gz1.w);

				float4 norm0 = taylorInvSqrt(float4(dot(g000, g000), dot(g010, g010), dot(g100, g100), dot(g110, g110)));
				g000 *= norm0.x;
				g010 *= norm0.y;
				g100 *= norm0.z;
				g110 *= norm0.w;
				float4 norm1 = taylorInvSqrt(float4(dot(g001, g001), dot(g011, g011), dot(g101, g101), dot(g111, g111)));
				g001 *= norm1.x;
				g011 *= norm1.y;
				g101 *= norm1.z;
				g111 *= norm1.w;

				float n000 = dot(g000, Pf0);
				float n100 = dot(g100, float3(Pf1.x, Pf0.yz));
				float n010 = dot(g010, float3(Pf0.x, Pf1.y, Pf0.z));
				float n110 = dot(g110, float3(Pf1.xy, Pf0.z));
				float n001 = dot(g001, float3(Pf0.xy, Pf1.z));
				float n101 = dot(g101, float3(Pf1.x, Pf0.y, Pf1.z));
				float n011 = dot(g011, float3(Pf0.x, Pf1.yz));
				float n111 = dot(g111, Pf1);

				float3 fade_xyz = fade(Pf0);
				float4 n_z = lerp(float4(n000, n100, n010, n110), float4(n001, n101, n011, n111), fade_xyz.z);
				float2 n_yz = lerp(n_z.xy, n_z.zw, fade_xyz.y);
				float n_xyz = lerp(n_yz.x, n_yz.y, fade_xyz.x);
				return 1.1 * n_xyz + 0.5;
			}

			struct vIn
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct vOut
			{
				float4 position : SV_POSITION;
				float4 texcoord : TEXCOORD0;
				float4 worldpos : TEXCOORD1;
				float4 diffusal : COLOR;
			};

			float4 _CrystalColor;
			float4 _SolidColor;
			float _Minimum;
			float _Falloff;
			float _Zoom;
			bool _UseDiffuse;
			float _Distance, _Kc, _Kl, _Kq;

			uniform float4 _LightColor0;

			vOut vert(vIn input)
			{
				vOut output;

				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				float3 normalDirection = normalize(
					mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				float3 lightDirection;
				float attenuation;

				if (0.0 == _WorldSpaceLightPos0.w) // Directional
				{
					attenuation = 1.0;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				}
				else // Other
				{
					float3 vertexToLightSource = _WorldSpaceLightPos0.xyz
						- mul(modelMatrix, input.vertex).xyz;
					float distance = length(vertexToLightSource);
					distance = distance / _Distance;
					attenuation = _Distance / (_Kc + distance * _Kl + distance * distance * _Kq);
					lightDirection = normalize(vertexToLightSource);
				}

				float3 diffuseReflection =
					attenuation * _LightColor0.rgb * max(0.0, dot(normalDirection, lightDirection));

				output.diffusal = float4(diffuseReflection, 1.0);
				output.texcoord = input.texcoord;
				output.position = UnityObjectToClipPos(input.vertex);
				output.worldpos = mul(unity_ObjectToWorld, input.vertex);
				return output;
			}

			float4 frag(vOut input) : COLOR
			{
				bool useDiffuse = _UseDiffuse;
				float4 color;
				float4 crystal = _CrystalColor;
				float4 solid = _SolidColor;
				float minimum = _Minimum;
				float falloff = _Falloff;
				float zoom = _Zoom;

				const float offset = 0.1;
				float perlin = 0.0;
/*
				for (int X = -1; X <= 1; X++)
				{
					for (int Y = -1; Y <= 1; Y++)
					{
						for (int Z = -1; Z <= 1; Z++)
						{
							int where = abs(X) + abs(Y) + abs(Z);
							int amount;
							if (where == 3)
								amount = -1;
							else if (where == 2)
								amount = -2;
							else if (where == 1)
								amount = -4;
							else
								amount = 8 * 1 + 12 * 2 + 6 * 4 + 1;
							float3 p = input.worldpos.xyz * zoom;
							p.x = p.x + X * offset;
							p.y = p.y + Y * offset;
							p.z = p.z + Z * offset;
							perlin += cnoise(p) * amount;
						}
					}
				}*/

				perlin = cnoise(input.worldpos.xyz * zoom);

				float fall = 1 + abs(perlin - minimum) * (falloff);

				if (perlin > minimum)
					color = (1 - perlin) * (2 / fall) * crystal;
				else
					color = solid;
				if (useDiffuse)
					return color * input.diffusal;
				else
					return color;
			}
			ENDCG
		}
	}
	FallBack "Crystal"
}
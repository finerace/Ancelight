Shader "WeaponsUIeffectMask"
{
    Properties
    {
        [NoScaleOffset]_TargetTexture("TargetTexture", 2D) = "white" {}
        [ToggleUI]_IsTransparentTexture("IsTransparentTexture", Float) = 0
        [NoScaleOffset]_TargetTexture2("TargetTexture2", 2D) = "white" {}
        [HDR]_MainColor("Color", Color) = (0, 0.6671038, 2, 1)
        _Intensity("Intensity", Float) = 1
        Vector1_cdea67384fbe4b94baa4476968e9f762("PlusAlpha", Float) = 0.25
        Vector2_b8d33165554f47859715f268ca93d9dd("Tiling", Vector) = (3, 3, 0, 0)
        _Speed("Speed", Float) = 0.5
        _Brightness("Brightness", Float) = 1
        [ToggleUI]_Disturbance("Disturbance", Float) = 0
        [NoScaleOffset]_MainTex("_MainTex", 2D) = "white" {}
        _UnscaledTime("UnscaledTime", Float) = 0
        _Rotation("Rotation", Float) = 0
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}

        _Stencil("Stencil ID", Float) = 0
        _StencilComp("StencilComp", Float) = 8
        _StencilOp("StencilOp", Float) = 0
        _StencilReadMask("StencilReadMask", Float) = 255
        _StencilWriteMask("StencilWriteMask", Float) = 255
        _ColorMask("ColorMask", Float) = 15
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Lit"
            "Queue"="Transparent"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"=""
        }
        Pass
        {
            Name "Sprite Lit"
            Tags
            {
                "LightMode" = "Universal2D"
            }
        
            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest [unity_GUIZTestMode]
        ZWrite Off
        
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]

            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>
        
            // Keywords
            #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_0
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_1
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_2
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
            // GraphKeywords: <None>
        
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define VARYINGS_NEED_SCREENPOSITION
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITELIT
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
            // Includes
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
             float4 screenPosition;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float4 interp2 : INTERP2;
             float4 interp3 : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyzw =  input.texCoord0;
            output.interp2.xyzw =  input.color;
            output.interp3.xyzw =  input.screenPosition;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.color = input.interp2.xyzw;
            output.screenPosition = input.interp3.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TargetTexture_TexelSize;
        float4 _TargetTexture2_TexelSize;
        float4 _MainColor;
        float _Intensity;
        float Vector1_cdea67384fbe4b94baa4476968e9f762;
        float2 Vector2_b8d33165554f47859715f268ca93d9dd;
        float _Speed;
        float _Brightness;
        float _Disturbance;
        float4 _MainTex_TexelSize;
        float _UnscaledTime;
        float _Rotation;
        float _IsTransparentTexture;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TargetTexture);
        SAMPLER(sampler_TargetTexture);
        TEXTURE2D(_TargetTexture2);
        SAMPLER(sampler_TargetTexture2);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
            // Graph Includes
            // GraphIncludes: <None>
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Graph Functions
            
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        
        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        {
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Branch_float3(float Predicate, float3 True, float3 False, out float3 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Property_5b7413479f844f1cb7bea80b77a3206c_Out_0 = _Disturbance;
            float2 _Vector2_56b07c3b01d74c2fb762b3665ba1834b_Out_0 = float2(0, 5);
            float2 _Multiply_0f3ce384361c4e13910378af5e11663f_Out_2;
            Unity_Multiply_float2_float2(_Vector2_56b07c3b01d74c2fb762b3665ba1834b_Out_0, (IN.TimeParameters.x.xx), _Multiply_0f3ce384361c4e13910378af5e11663f_Out_2);
            float2 _TilingAndOffset_a2e8ba11d3fd46edb1150ebff07cc875_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (0.01, 1.4), _Multiply_0f3ce384361c4e13910378af5e11663f_Out_2, _TilingAndOffset_a2e8ba11d3fd46edb1150ebff07cc875_Out_3);
            float _GradientNoise_0ee7d9f541974d788d323c1bde3e7e00_Out_2;
            Unity_GradientNoise_float(_TilingAndOffset_a2e8ba11d3fd46edb1150ebff07cc875_Out_3, 10, _GradientNoise_0ee7d9f541974d788d323c1bde3e7e00_Out_2);
            float _InvertColors_820c98bdc41044df961a0881737b2567_Out_1;
            float _InvertColors_820c98bdc41044df961a0881737b2567_InvertColors = float (1);
            Unity_InvertColors_float(_GradientNoise_0ee7d9f541974d788d323c1bde3e7e00_Out_2, _InvertColors_820c98bdc41044df961a0881737b2567_InvertColors, _InvertColors_820c98bdc41044df961a0881737b2567_Out_1);
            float _Remap_933037f6624f4538b1d07f8c3a76a5ed_Out_3;
            Unity_Remap_float(_InvertColors_820c98bdc41044df961a0881737b2567_Out_1, float2 (-1, 1), float2 (0, 1), _Remap_933037f6624f4538b1d07f8c3a76a5ed_Out_3);
            float _Add_fe38576d18f64ce0b764c66710b47170_Out_2;
            Unity_Add_float(_Remap_933037f6624f4538b1d07f8c3a76a5ed_Out_3, 0.35, _Add_fe38576d18f64ce0b764c66710b47170_Out_2);
            float _Clamp_7ef923a3f14948b4941a8510ef26c2ea_Out_3;
            Unity_Clamp_float(_Add_fe38576d18f64ce0b764c66710b47170_Out_2, 0.98, 1, _Clamp_7ef923a3f14948b4941a8510ef26c2ea_Out_3);
            float3 _Multiply_ff1b071c34424b1a9e402ba831fcdf64_Out_2;
            Unity_Multiply_float3_float3(IN.ObjectSpacePosition, (_Clamp_7ef923a3f14948b4941a8510ef26c2ea_Out_3.xxx), _Multiply_ff1b071c34424b1a9e402ba831fcdf64_Out_2);
            float3 _Branch_4ecfd0be62ab47738166d09208a68fa4_Out_3;
            Unity_Branch_float3(_Property_5b7413479f844f1cb7bea80b77a3206c_Out_0, _Multiply_ff1b071c34424b1a9e402ba831fcdf64_Out_2, IN.ObjectSpacePosition, _Branch_4ecfd0be62ab47738166d09208a68fa4_Out_3);
            description.Position = _Branch_4ecfd0be62ab47738166d09208a68fa4_Out_3;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float4 SpriteMask;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float2 _Property_d52ce9ecd75b4bad89952518448a18bd_Out_0 = Vector2_b8d33165554f47859715f268ca93d9dd;
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_R_1 = _Property_d52ce9ecd75b4bad89952518448a18bd_Out_0[0];
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_G_2 = _Property_d52ce9ecd75b4bad89952518448a18bd_Out_0[1];
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_B_3 = 0;
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_A_4 = 0;
            float2 _Vector2_5caa6ab780d54dd099c9b36d34618d6f_Out_0 = float2(0, _Split_f615a84a71594c0fbccf5f1cd8ff52d2_G_2);
            float _Property_bfd4fac952864b44b810378fc1d799d2_Out_0 = _Speed;
            float2 _Vector2_9f3dd73887a641b9ac56504bf68ae5ba_Out_0 = float2(0, _Property_bfd4fac952864b44b810378fc1d799d2_Out_0);
            float _Property_218ab6af02df441aa7e739c2142ea8f6_Out_0 = _UnscaledTime;
            float _Add_e624e733158949be9b27794d1bb13482_Out_2;
            Unity_Add_float(IN.TimeParameters.x, _Property_218ab6af02df441aa7e739c2142ea8f6_Out_0, _Add_e624e733158949be9b27794d1bb13482_Out_2);
            float2 _Multiply_f8718557c7ca47b8b3fc39d36d09ed13_Out_2;
            Unity_Multiply_float2_float2(_Vector2_9f3dd73887a641b9ac56504bf68ae5ba_Out_0, (_Add_e624e733158949be9b27794d1bb13482_Out_2.xx), _Multiply_f8718557c7ca47b8b3fc39d36d09ed13_Out_2);
            float2 _TilingAndOffset_b0546cddf2a148a1a52847c96ea74e8b_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Vector2_5caa6ab780d54dd099c9b36d34618d6f_Out_0, _Multiply_f8718557c7ca47b8b3fc39d36d09ed13_Out_2, _TilingAndOffset_b0546cddf2a148a1a52847c96ea74e8b_Out_3);
            float _GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2;
            Unity_GradientNoise_float(_TilingAndOffset_b0546cddf2a148a1a52847c96ea74e8b_Out_3, 10, _GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2);
            float _InvertColors_18445188fb7e4b59a2f8eab3c047837c_Out_1;
            float _InvertColors_18445188fb7e4b59a2f8eab3c047837c_InvertColors = float (1);
            Unity_InvertColors_float(_GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2, _InvertColors_18445188fb7e4b59a2f8eab3c047837c_InvertColors, _InvertColors_18445188fb7e4b59a2f8eab3c047837c_Out_1);
            float _Property_a57dcd5331a1400ba6f1c8c70db49e2f_Out_0 = _IsTransparentTexture;
            UnityTexture2D _Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0 = UnityBuildTexture2DStructNoScale(_TargetTexture2);
            float4 _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0.tex, _Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0.samplerstate, _Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_R_4 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.r;
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_G_5 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.g;
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_B_6 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.b;
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_A_7 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.a;
            float _Split_9648a8090aa14ce0abb7445e21241c43_R_1 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[0];
            float _Split_9648a8090aa14ce0abb7445e21241c43_G_2 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[1];
            float _Split_9648a8090aa14ce0abb7445e21241c43_B_3 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[2];
            float _Split_9648a8090aa14ce0abb7445e21241c43_A_4 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[3];
            float _Add_3d5c77838ac34011b65ca68f063f5dd4_Out_2;
            Unity_Add_float(_Split_9648a8090aa14ce0abb7445e21241c43_A_4, 0, _Add_3d5c77838ac34011b65ca68f063f5dd4_Out_2);
            UnityTexture2D _Property_4ae289f5d240471ca9e6399a761815f7_Out_0 = UnityBuildTexture2DStructNoScale(_TargetTexture);
            float4 _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0 = SAMPLE_TEXTURE2D(_Property_4ae289f5d240471ca9e6399a761815f7_Out_0.tex, _Property_4ae289f5d240471ca9e6399a761815f7_Out_0.samplerstate, _Property_4ae289f5d240471ca9e6399a761815f7_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_R_4 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.r;
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_G_5 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.g;
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_B_6 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.b;
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_A_7 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.a;
            float4 _Branch_88817652b5b749c2b29dd2e4f32d135b_Out_3;
            Unity_Branch_float4(_Property_a57dcd5331a1400ba6f1c8c70db49e2f_Out_0, (_Add_3d5c77838ac34011b65ca68f063f5dd4_Out_2.xxxx), _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0, _Branch_88817652b5b749c2b29dd2e4f32d135b_Out_3);
            float _Property_52a7d19f4bd843458ef1288ac990f460_Out_0 = _Brightness;
            float4 _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2;
            Unity_Multiply_float4_float4(_Branch_88817652b5b749c2b29dd2e4f32d135b_Out_3, (_Property_52a7d19f4bd843458ef1288ac990f460_Out_0.xxxx), _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2);
            float4 _Multiply_420383d8fff643baac82888de6defe00_Out_2;
            Unity_Multiply_float4_float4((_InvertColors_18445188fb7e4b59a2f8eab3c047837c_Out_1.xxxx), _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2, _Multiply_420383d8fff643baac82888de6defe00_Out_2);
            float _Property_489ef5838aaf42f19939756c5a5c1f87_Out_0 = Vector1_cdea67384fbe4b94baa4476968e9f762;
            float _Multiply_4c94c05e6cb444119157b17ebdc161e0_Out_2;
            Unity_Multiply_float_float(_GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2, _Property_489ef5838aaf42f19939756c5a5c1f87_Out_0, _Multiply_4c94c05e6cb444119157b17ebdc161e0_Out_2);
            float4 _Multiply_638ee8b9bb1b4ba28fd703a3fc89ee32_Out_2;
            Unity_Multiply_float4_float4((_Multiply_4c94c05e6cb444119157b17ebdc161e0_Out_2.xxxx), _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2, _Multiply_638ee8b9bb1b4ba28fd703a3fc89ee32_Out_2);
            float4 _Add_d0dacea2b508410a8ff9d4af2127f57f_Out_2;
            Unity_Add_float4(_Multiply_420383d8fff643baac82888de6defe00_Out_2, _Multiply_638ee8b9bb1b4ba28fd703a3fc89ee32_Out_2, _Add_d0dacea2b508410a8ff9d4af2127f57f_Out_2);
            float4 _Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2;
            Unity_Multiply_float4_float4(_Add_d0dacea2b508410a8ff9d4af2127f57f_Out_2, float4(1, 1, 1, 1), _Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2);
            float4 _Property_99171730b95a4aa7b036e0b6a914c3b0_Out_0 = IsGammaSpace() ? LinearToSRGB(_MainColor) : _MainColor;
            float4 _Multiply_64d3b80fdaec4e859cdf686d4db3b57a_Out_2;
            Unity_Multiply_float4_float4(_Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2, _Property_99171730b95a4aa7b036e0b6a914c3b0_Out_0, _Multiply_64d3b80fdaec4e859cdf686d4db3b57a_Out_2);
            float _Property_1900347b72814f478881370b66df9ba3_Out_0 = _Intensity;
            float4 _Multiply_d562341f791c4c0ebc86e80540330ecd_Out_2;
            Unity_Multiply_float4_float4(_Multiply_64d3b80fdaec4e859cdf686d4db3b57a_Out_2, (_Property_1900347b72814f478881370b66df9ba3_Out_0.xxxx), _Multiply_d562341f791c4c0ebc86e80540330ecd_Out_2);
            surface.BaseColor = (_Multiply_d562341f791c4c0ebc86e80540330ecd_Out_2.xyz);
            surface.Alpha = (_Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2).x;
            surface.SpriteMask = IsGammaSpace() ? float4(1, 1, 1, 1) : float4 (SRGBToLinear(float3(1, 1, 1)), 1);
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.uv0 =                                        input.uv0;
            output.TimeParameters =                             _TimeParameters.xyz;
        
            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 =                                        input.texCoord0;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteLitPass.hlsl"
        
            ENDHLSL
        }
        Pass
        {
            Name "Sprite Normal"
            Tags
            {
                "LightMode" = "NormalsRendering"
            }
        
            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest [unity_GUIZTestMode]
        ZWrite Off
        

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]

            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
        
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITENORMAL
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
            // Includes
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float4 interp2 : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.tangentWS;
            output.interp2.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.tangentWS = input.interp1.xyzw;
            output.texCoord0 = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TargetTexture_TexelSize;
        float4 _TargetTexture2_TexelSize;
        float4 _MainColor;
        float _Intensity;
        float Vector1_cdea67384fbe4b94baa4476968e9f762;
        float2 Vector2_b8d33165554f47859715f268ca93d9dd;
        float _Speed;
        float _Brightness;
        float _Disturbance;
        float4 _MainTex_TexelSize;
        float _UnscaledTime;
        float _Rotation;
        float _IsTransparentTexture;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TargetTexture);
        SAMPLER(sampler_TargetTexture);
        TEXTURE2D(_TargetTexture2);
        SAMPLER(sampler_TargetTexture2);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
            // Graph Includes
            // GraphIncludes: <None>
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Graph Functions
            
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        
        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        {
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Branch_float3(float Predicate, float3 True, float3 False, out float3 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Property_5b7413479f844f1cb7bea80b77a3206c_Out_0 = _Disturbance;
            float2 _Vector2_56b07c3b01d74c2fb762b3665ba1834b_Out_0 = float2(0, 5);
            float2 _Multiply_0f3ce384361c4e13910378af5e11663f_Out_2;
            Unity_Multiply_float2_float2(_Vector2_56b07c3b01d74c2fb762b3665ba1834b_Out_0, (IN.TimeParameters.x.xx), _Multiply_0f3ce384361c4e13910378af5e11663f_Out_2);
            float2 _TilingAndOffset_a2e8ba11d3fd46edb1150ebff07cc875_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (0.01, 1.4), _Multiply_0f3ce384361c4e13910378af5e11663f_Out_2, _TilingAndOffset_a2e8ba11d3fd46edb1150ebff07cc875_Out_3);
            float _GradientNoise_0ee7d9f541974d788d323c1bde3e7e00_Out_2;
            Unity_GradientNoise_float(_TilingAndOffset_a2e8ba11d3fd46edb1150ebff07cc875_Out_3, 10, _GradientNoise_0ee7d9f541974d788d323c1bde3e7e00_Out_2);
            float _InvertColors_820c98bdc41044df961a0881737b2567_Out_1;
            float _InvertColors_820c98bdc41044df961a0881737b2567_InvertColors = float (1);
            Unity_InvertColors_float(_GradientNoise_0ee7d9f541974d788d323c1bde3e7e00_Out_2, _InvertColors_820c98bdc41044df961a0881737b2567_InvertColors, _InvertColors_820c98bdc41044df961a0881737b2567_Out_1);
            float _Remap_933037f6624f4538b1d07f8c3a76a5ed_Out_3;
            Unity_Remap_float(_InvertColors_820c98bdc41044df961a0881737b2567_Out_1, float2 (-1, 1), float2 (0, 1), _Remap_933037f6624f4538b1d07f8c3a76a5ed_Out_3);
            float _Add_fe38576d18f64ce0b764c66710b47170_Out_2;
            Unity_Add_float(_Remap_933037f6624f4538b1d07f8c3a76a5ed_Out_3, 0.35, _Add_fe38576d18f64ce0b764c66710b47170_Out_2);
            float _Clamp_7ef923a3f14948b4941a8510ef26c2ea_Out_3;
            Unity_Clamp_float(_Add_fe38576d18f64ce0b764c66710b47170_Out_2, 0.98, 1, _Clamp_7ef923a3f14948b4941a8510ef26c2ea_Out_3);
            float3 _Multiply_ff1b071c34424b1a9e402ba831fcdf64_Out_2;
            Unity_Multiply_float3_float3(IN.ObjectSpacePosition, (_Clamp_7ef923a3f14948b4941a8510ef26c2ea_Out_3.xxx), _Multiply_ff1b071c34424b1a9e402ba831fcdf64_Out_2);
            float3 _Branch_4ecfd0be62ab47738166d09208a68fa4_Out_3;
            Unity_Branch_float3(_Property_5b7413479f844f1cb7bea80b77a3206c_Out_0, _Multiply_ff1b071c34424b1a9e402ba831fcdf64_Out_2, IN.ObjectSpacePosition, _Branch_4ecfd0be62ab47738166d09208a68fa4_Out_3);
            description.Position = _Branch_4ecfd0be62ab47738166d09208a68fa4_Out_3;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float2 _Property_d52ce9ecd75b4bad89952518448a18bd_Out_0 = Vector2_b8d33165554f47859715f268ca93d9dd;
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_R_1 = _Property_d52ce9ecd75b4bad89952518448a18bd_Out_0[0];
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_G_2 = _Property_d52ce9ecd75b4bad89952518448a18bd_Out_0[1];
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_B_3 = 0;
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_A_4 = 0;
            float2 _Vector2_5caa6ab780d54dd099c9b36d34618d6f_Out_0 = float2(0, _Split_f615a84a71594c0fbccf5f1cd8ff52d2_G_2);
            float _Property_bfd4fac952864b44b810378fc1d799d2_Out_0 = _Speed;
            float2 _Vector2_9f3dd73887a641b9ac56504bf68ae5ba_Out_0 = float2(0, _Property_bfd4fac952864b44b810378fc1d799d2_Out_0);
            float _Property_218ab6af02df441aa7e739c2142ea8f6_Out_0 = _UnscaledTime;
            float _Add_e624e733158949be9b27794d1bb13482_Out_2;
            Unity_Add_float(IN.TimeParameters.x, _Property_218ab6af02df441aa7e739c2142ea8f6_Out_0, _Add_e624e733158949be9b27794d1bb13482_Out_2);
            float2 _Multiply_f8718557c7ca47b8b3fc39d36d09ed13_Out_2;
            Unity_Multiply_float2_float2(_Vector2_9f3dd73887a641b9ac56504bf68ae5ba_Out_0, (_Add_e624e733158949be9b27794d1bb13482_Out_2.xx), _Multiply_f8718557c7ca47b8b3fc39d36d09ed13_Out_2);
            float2 _TilingAndOffset_b0546cddf2a148a1a52847c96ea74e8b_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Vector2_5caa6ab780d54dd099c9b36d34618d6f_Out_0, _Multiply_f8718557c7ca47b8b3fc39d36d09ed13_Out_2, _TilingAndOffset_b0546cddf2a148a1a52847c96ea74e8b_Out_3);
            float _GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2;
            Unity_GradientNoise_float(_TilingAndOffset_b0546cddf2a148a1a52847c96ea74e8b_Out_3, 10, _GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2);
            float _InvertColors_18445188fb7e4b59a2f8eab3c047837c_Out_1;
            float _InvertColors_18445188fb7e4b59a2f8eab3c047837c_InvertColors = float (1);
            Unity_InvertColors_float(_GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2, _InvertColors_18445188fb7e4b59a2f8eab3c047837c_InvertColors, _InvertColors_18445188fb7e4b59a2f8eab3c047837c_Out_1);
            float _Property_a57dcd5331a1400ba6f1c8c70db49e2f_Out_0 = _IsTransparentTexture;
            UnityTexture2D _Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0 = UnityBuildTexture2DStructNoScale(_TargetTexture2);
            float4 _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0.tex, _Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0.samplerstate, _Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_R_4 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.r;
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_G_5 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.g;
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_B_6 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.b;
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_A_7 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.a;
            float _Split_9648a8090aa14ce0abb7445e21241c43_R_1 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[0];
            float _Split_9648a8090aa14ce0abb7445e21241c43_G_2 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[1];
            float _Split_9648a8090aa14ce0abb7445e21241c43_B_3 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[2];
            float _Split_9648a8090aa14ce0abb7445e21241c43_A_4 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[3];
            float _Add_3d5c77838ac34011b65ca68f063f5dd4_Out_2;
            Unity_Add_float(_Split_9648a8090aa14ce0abb7445e21241c43_A_4, 0, _Add_3d5c77838ac34011b65ca68f063f5dd4_Out_2);
            UnityTexture2D _Property_4ae289f5d240471ca9e6399a761815f7_Out_0 = UnityBuildTexture2DStructNoScale(_TargetTexture);
            float4 _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0 = SAMPLE_TEXTURE2D(_Property_4ae289f5d240471ca9e6399a761815f7_Out_0.tex, _Property_4ae289f5d240471ca9e6399a761815f7_Out_0.samplerstate, _Property_4ae289f5d240471ca9e6399a761815f7_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_R_4 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.r;
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_G_5 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.g;
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_B_6 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.b;
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_A_7 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.a;
            float4 _Branch_88817652b5b749c2b29dd2e4f32d135b_Out_3;
            Unity_Branch_float4(_Property_a57dcd5331a1400ba6f1c8c70db49e2f_Out_0, (_Add_3d5c77838ac34011b65ca68f063f5dd4_Out_2.xxxx), _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0, _Branch_88817652b5b749c2b29dd2e4f32d135b_Out_3);
            float _Property_52a7d19f4bd843458ef1288ac990f460_Out_0 = _Brightness;
            float4 _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2;
            Unity_Multiply_float4_float4(_Branch_88817652b5b749c2b29dd2e4f32d135b_Out_3, (_Property_52a7d19f4bd843458ef1288ac990f460_Out_0.xxxx), _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2);
            float4 _Multiply_420383d8fff643baac82888de6defe00_Out_2;
            Unity_Multiply_float4_float4((_InvertColors_18445188fb7e4b59a2f8eab3c047837c_Out_1.xxxx), _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2, _Multiply_420383d8fff643baac82888de6defe00_Out_2);
            float _Property_489ef5838aaf42f19939756c5a5c1f87_Out_0 = Vector1_cdea67384fbe4b94baa4476968e9f762;
            float _Multiply_4c94c05e6cb444119157b17ebdc161e0_Out_2;
            Unity_Multiply_float_float(_GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2, _Property_489ef5838aaf42f19939756c5a5c1f87_Out_0, _Multiply_4c94c05e6cb444119157b17ebdc161e0_Out_2);
            float4 _Multiply_638ee8b9bb1b4ba28fd703a3fc89ee32_Out_2;
            Unity_Multiply_float4_float4((_Multiply_4c94c05e6cb444119157b17ebdc161e0_Out_2.xxxx), _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2, _Multiply_638ee8b9bb1b4ba28fd703a3fc89ee32_Out_2);
            float4 _Add_d0dacea2b508410a8ff9d4af2127f57f_Out_2;
            Unity_Add_float4(_Multiply_420383d8fff643baac82888de6defe00_Out_2, _Multiply_638ee8b9bb1b4ba28fd703a3fc89ee32_Out_2, _Add_d0dacea2b508410a8ff9d4af2127f57f_Out_2);
            float4 _Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2;
            Unity_Multiply_float4_float4(_Add_d0dacea2b508410a8ff9d4af2127f57f_Out_2, float4(1, 1, 1, 1), _Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2);
            float4 _Property_99171730b95a4aa7b036e0b6a914c3b0_Out_0 = IsGammaSpace() ? LinearToSRGB(_MainColor) : _MainColor;
            float4 _Multiply_64d3b80fdaec4e859cdf686d4db3b57a_Out_2;
            Unity_Multiply_float4_float4(_Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2, _Property_99171730b95a4aa7b036e0b6a914c3b0_Out_0, _Multiply_64d3b80fdaec4e859cdf686d4db3b57a_Out_2);
            float _Property_1900347b72814f478881370b66df9ba3_Out_0 = _Intensity;
            float4 _Multiply_d562341f791c4c0ebc86e80540330ecd_Out_2;
            Unity_Multiply_float4_float4(_Multiply_64d3b80fdaec4e859cdf686d4db3b57a_Out_2, (_Property_1900347b72814f478881370b66df9ba3_Out_0.xxxx), _Multiply_d562341f791c4c0ebc86e80540330ecd_Out_2);
            surface.BaseColor = (_Multiply_d562341f791c4c0ebc86e80540330ecd_Out_2.xyz);
            surface.Alpha = (_Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2).x;
            surface.NormalTS = IN.TangentSpaceNormal;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.uv0 =                                        input.uv0;
            output.TimeParameters =                             _TimeParameters.xyz;
        
            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
            output.TangentSpaceNormal =                         float3(0.0f, 0.0f, 1.0f);
        
        
            output.uv0 =                                        input.texCoord0;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteNormalPass.hlsl"
        
            ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
            ZTest [unity_GUIZTestMode]
            // Render State
            Cull Off
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]

            HLSLPROGRAM
        
            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
        
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD0
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
            // Includes
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 interp0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TargetTexture_TexelSize;
        float4 _TargetTexture2_TexelSize;
        float4 _MainColor;
        float _Intensity;
        float Vector1_cdea67384fbe4b94baa4476968e9f762;
        float2 Vector2_b8d33165554f47859715f268ca93d9dd;
        float _Speed;
        float _Brightness;
        float _Disturbance;
        float4 _MainTex_TexelSize;
        float _UnscaledTime;
        float _Rotation;
        float _IsTransparentTexture;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TargetTexture);
        SAMPLER(sampler_TargetTexture);
        TEXTURE2D(_TargetTexture2);
        SAMPLER(sampler_TargetTexture2);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
            // Graph Includes
            // GraphIncludes: <None>
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Graph Functions
            
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        
        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        {
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Branch_float3(float Predicate, float3 True, float3 False, out float3 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Property_5b7413479f844f1cb7bea80b77a3206c_Out_0 = _Disturbance;
            float2 _Vector2_56b07c3b01d74c2fb762b3665ba1834b_Out_0 = float2(0, 5);
            float2 _Multiply_0f3ce384361c4e13910378af5e11663f_Out_2;
            Unity_Multiply_float2_float2(_Vector2_56b07c3b01d74c2fb762b3665ba1834b_Out_0, (IN.TimeParameters.x.xx), _Multiply_0f3ce384361c4e13910378af5e11663f_Out_2);
            float2 _TilingAndOffset_a2e8ba11d3fd46edb1150ebff07cc875_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (0.01, 1.4), _Multiply_0f3ce384361c4e13910378af5e11663f_Out_2, _TilingAndOffset_a2e8ba11d3fd46edb1150ebff07cc875_Out_3);
            float _GradientNoise_0ee7d9f541974d788d323c1bde3e7e00_Out_2;
            Unity_GradientNoise_float(_TilingAndOffset_a2e8ba11d3fd46edb1150ebff07cc875_Out_3, 10, _GradientNoise_0ee7d9f541974d788d323c1bde3e7e00_Out_2);
            float _InvertColors_820c98bdc41044df961a0881737b2567_Out_1;
            float _InvertColors_820c98bdc41044df961a0881737b2567_InvertColors = float (1);
            Unity_InvertColors_float(_GradientNoise_0ee7d9f541974d788d323c1bde3e7e00_Out_2, _InvertColors_820c98bdc41044df961a0881737b2567_InvertColors, _InvertColors_820c98bdc41044df961a0881737b2567_Out_1);
            float _Remap_933037f6624f4538b1d07f8c3a76a5ed_Out_3;
            Unity_Remap_float(_InvertColors_820c98bdc41044df961a0881737b2567_Out_1, float2 (-1, 1), float2 (0, 1), _Remap_933037f6624f4538b1d07f8c3a76a5ed_Out_3);
            float _Add_fe38576d18f64ce0b764c66710b47170_Out_2;
            Unity_Add_float(_Remap_933037f6624f4538b1d07f8c3a76a5ed_Out_3, 0.35, _Add_fe38576d18f64ce0b764c66710b47170_Out_2);
            float _Clamp_7ef923a3f14948b4941a8510ef26c2ea_Out_3;
            Unity_Clamp_float(_Add_fe38576d18f64ce0b764c66710b47170_Out_2, 0.98, 1, _Clamp_7ef923a3f14948b4941a8510ef26c2ea_Out_3);
            float3 _Multiply_ff1b071c34424b1a9e402ba831fcdf64_Out_2;
            Unity_Multiply_float3_float3(IN.ObjectSpacePosition, (_Clamp_7ef923a3f14948b4941a8510ef26c2ea_Out_3.xxx), _Multiply_ff1b071c34424b1a9e402ba831fcdf64_Out_2);
            float3 _Branch_4ecfd0be62ab47738166d09208a68fa4_Out_3;
            Unity_Branch_float3(_Property_5b7413479f844f1cb7bea80b77a3206c_Out_0, _Multiply_ff1b071c34424b1a9e402ba831fcdf64_Out_2, IN.ObjectSpacePosition, _Branch_4ecfd0be62ab47738166d09208a68fa4_Out_3);
            description.Position = _Branch_4ecfd0be62ab47738166d09208a68fa4_Out_3;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float2 _Property_d52ce9ecd75b4bad89952518448a18bd_Out_0 = Vector2_b8d33165554f47859715f268ca93d9dd;
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_R_1 = _Property_d52ce9ecd75b4bad89952518448a18bd_Out_0[0];
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_G_2 = _Property_d52ce9ecd75b4bad89952518448a18bd_Out_0[1];
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_B_3 = 0;
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_A_4 = 0;
            float2 _Vector2_5caa6ab780d54dd099c9b36d34618d6f_Out_0 = float2(0, _Split_f615a84a71594c0fbccf5f1cd8ff52d2_G_2);
            float _Property_bfd4fac952864b44b810378fc1d799d2_Out_0 = _Speed;
            float2 _Vector2_9f3dd73887a641b9ac56504bf68ae5ba_Out_0 = float2(0, _Property_bfd4fac952864b44b810378fc1d799d2_Out_0);
            float _Property_218ab6af02df441aa7e739c2142ea8f6_Out_0 = _UnscaledTime;
            float _Add_e624e733158949be9b27794d1bb13482_Out_2;
            Unity_Add_float(IN.TimeParameters.x, _Property_218ab6af02df441aa7e739c2142ea8f6_Out_0, _Add_e624e733158949be9b27794d1bb13482_Out_2);
            float2 _Multiply_f8718557c7ca47b8b3fc39d36d09ed13_Out_2;
            Unity_Multiply_float2_float2(_Vector2_9f3dd73887a641b9ac56504bf68ae5ba_Out_0, (_Add_e624e733158949be9b27794d1bb13482_Out_2.xx), _Multiply_f8718557c7ca47b8b3fc39d36d09ed13_Out_2);
            float2 _TilingAndOffset_b0546cddf2a148a1a52847c96ea74e8b_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Vector2_5caa6ab780d54dd099c9b36d34618d6f_Out_0, _Multiply_f8718557c7ca47b8b3fc39d36d09ed13_Out_2, _TilingAndOffset_b0546cddf2a148a1a52847c96ea74e8b_Out_3);
            float _GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2;
            Unity_GradientNoise_float(_TilingAndOffset_b0546cddf2a148a1a52847c96ea74e8b_Out_3, 10, _GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2);
            float _InvertColors_18445188fb7e4b59a2f8eab3c047837c_Out_1;
            float _InvertColors_18445188fb7e4b59a2f8eab3c047837c_InvertColors = float (1);
            Unity_InvertColors_float(_GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2, _InvertColors_18445188fb7e4b59a2f8eab3c047837c_InvertColors, _InvertColors_18445188fb7e4b59a2f8eab3c047837c_Out_1);
            float _Property_a57dcd5331a1400ba6f1c8c70db49e2f_Out_0 = _IsTransparentTexture;
            UnityTexture2D _Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0 = UnityBuildTexture2DStructNoScale(_TargetTexture2);
            float4 _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0.tex, _Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0.samplerstate, _Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_R_4 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.r;
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_G_5 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.g;
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_B_6 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.b;
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_A_7 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.a;
            float _Split_9648a8090aa14ce0abb7445e21241c43_R_1 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[0];
            float _Split_9648a8090aa14ce0abb7445e21241c43_G_2 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[1];
            float _Split_9648a8090aa14ce0abb7445e21241c43_B_3 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[2];
            float _Split_9648a8090aa14ce0abb7445e21241c43_A_4 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[3];
            float _Add_3d5c77838ac34011b65ca68f063f5dd4_Out_2;
            Unity_Add_float(_Split_9648a8090aa14ce0abb7445e21241c43_A_4, 0, _Add_3d5c77838ac34011b65ca68f063f5dd4_Out_2);
            UnityTexture2D _Property_4ae289f5d240471ca9e6399a761815f7_Out_0 = UnityBuildTexture2DStructNoScale(_TargetTexture);
            float4 _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0 = SAMPLE_TEXTURE2D(_Property_4ae289f5d240471ca9e6399a761815f7_Out_0.tex, _Property_4ae289f5d240471ca9e6399a761815f7_Out_0.samplerstate, _Property_4ae289f5d240471ca9e6399a761815f7_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_R_4 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.r;
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_G_5 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.g;
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_B_6 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.b;
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_A_7 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.a;
            float4 _Branch_88817652b5b749c2b29dd2e4f32d135b_Out_3;
            Unity_Branch_float4(_Property_a57dcd5331a1400ba6f1c8c70db49e2f_Out_0, (_Add_3d5c77838ac34011b65ca68f063f5dd4_Out_2.xxxx), _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0, _Branch_88817652b5b749c2b29dd2e4f32d135b_Out_3);
            float _Property_52a7d19f4bd843458ef1288ac990f460_Out_0 = _Brightness;
            float4 _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2;
            Unity_Multiply_float4_float4(_Branch_88817652b5b749c2b29dd2e4f32d135b_Out_3, (_Property_52a7d19f4bd843458ef1288ac990f460_Out_0.xxxx), _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2);
            float4 _Multiply_420383d8fff643baac82888de6defe00_Out_2;
            Unity_Multiply_float4_float4((_InvertColors_18445188fb7e4b59a2f8eab3c047837c_Out_1.xxxx), _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2, _Multiply_420383d8fff643baac82888de6defe00_Out_2);
            float _Property_489ef5838aaf42f19939756c5a5c1f87_Out_0 = Vector1_cdea67384fbe4b94baa4476968e9f762;
            float _Multiply_4c94c05e6cb444119157b17ebdc161e0_Out_2;
            Unity_Multiply_float_float(_GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2, _Property_489ef5838aaf42f19939756c5a5c1f87_Out_0, _Multiply_4c94c05e6cb444119157b17ebdc161e0_Out_2);
            float4 _Multiply_638ee8b9bb1b4ba28fd703a3fc89ee32_Out_2;
            Unity_Multiply_float4_float4((_Multiply_4c94c05e6cb444119157b17ebdc161e0_Out_2.xxxx), _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2, _Multiply_638ee8b9bb1b4ba28fd703a3fc89ee32_Out_2);
            float4 _Add_d0dacea2b508410a8ff9d4af2127f57f_Out_2;
            Unity_Add_float4(_Multiply_420383d8fff643baac82888de6defe00_Out_2, _Multiply_638ee8b9bb1b4ba28fd703a3fc89ee32_Out_2, _Add_d0dacea2b508410a8ff9d4af2127f57f_Out_2);
            float4 _Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2;
            Unity_Multiply_float4_float4(_Add_d0dacea2b508410a8ff9d4af2127f57f_Out_2, float4(1, 1, 1, 1), _Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2);
            surface.Alpha = (_Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2).x;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.uv0 =                                        input.uv0;
            output.TimeParameters =                             _TimeParameters.xyz;
        
            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 =                                        input.texCoord0;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
            ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
            // Render State
            Cull Back
            ZTest [unity_GUIZTestMode]

            Stencil
            {
                Ref [_Stencil]
                Comp [_StencilComp]
                Pass [_StencilOp]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
            }
            ColorMask [_ColorMask]
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
        
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD0
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
            // Includes
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 interp0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TargetTexture_TexelSize;
        float4 _TargetTexture2_TexelSize;
        float4 _MainColor;
        float _Intensity;
        float Vector1_cdea67384fbe4b94baa4476968e9f762;
        float2 Vector2_b8d33165554f47859715f268ca93d9dd;
        float _Speed;
        float _Brightness;
        float _Disturbance;
        float4 _MainTex_TexelSize;
        float _UnscaledTime;
        float _Rotation;
        float _IsTransparentTexture;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TargetTexture);
        SAMPLER(sampler_TargetTexture);
        TEXTURE2D(_TargetTexture2);
        SAMPLER(sampler_TargetTexture2);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
            // Graph Includes
            // GraphIncludes: <None>
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Graph Functions
            
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        
        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        {
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Branch_float3(float Predicate, float3 True, float3 False, out float3 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Property_5b7413479f844f1cb7bea80b77a3206c_Out_0 = _Disturbance;
            float2 _Vector2_56b07c3b01d74c2fb762b3665ba1834b_Out_0 = float2(0, 5);
            float2 _Multiply_0f3ce384361c4e13910378af5e11663f_Out_2;
            Unity_Multiply_float2_float2(_Vector2_56b07c3b01d74c2fb762b3665ba1834b_Out_0, (IN.TimeParameters.x.xx), _Multiply_0f3ce384361c4e13910378af5e11663f_Out_2);
            float2 _TilingAndOffset_a2e8ba11d3fd46edb1150ebff07cc875_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (0.01, 1.4), _Multiply_0f3ce384361c4e13910378af5e11663f_Out_2, _TilingAndOffset_a2e8ba11d3fd46edb1150ebff07cc875_Out_3);
            float _GradientNoise_0ee7d9f541974d788d323c1bde3e7e00_Out_2;
            Unity_GradientNoise_float(_TilingAndOffset_a2e8ba11d3fd46edb1150ebff07cc875_Out_3, 10, _GradientNoise_0ee7d9f541974d788d323c1bde3e7e00_Out_2);
            float _InvertColors_820c98bdc41044df961a0881737b2567_Out_1;
            float _InvertColors_820c98bdc41044df961a0881737b2567_InvertColors = float (1);
            Unity_InvertColors_float(_GradientNoise_0ee7d9f541974d788d323c1bde3e7e00_Out_2, _InvertColors_820c98bdc41044df961a0881737b2567_InvertColors, _InvertColors_820c98bdc41044df961a0881737b2567_Out_1);
            float _Remap_933037f6624f4538b1d07f8c3a76a5ed_Out_3;
            Unity_Remap_float(_InvertColors_820c98bdc41044df961a0881737b2567_Out_1, float2 (-1, 1), float2 (0, 1), _Remap_933037f6624f4538b1d07f8c3a76a5ed_Out_3);
            float _Add_fe38576d18f64ce0b764c66710b47170_Out_2;
            Unity_Add_float(_Remap_933037f6624f4538b1d07f8c3a76a5ed_Out_3, 0.35, _Add_fe38576d18f64ce0b764c66710b47170_Out_2);
            float _Clamp_7ef923a3f14948b4941a8510ef26c2ea_Out_3;
            Unity_Clamp_float(_Add_fe38576d18f64ce0b764c66710b47170_Out_2, 0.98, 1, _Clamp_7ef923a3f14948b4941a8510ef26c2ea_Out_3);
            float3 _Multiply_ff1b071c34424b1a9e402ba831fcdf64_Out_2;
            Unity_Multiply_float3_float3(IN.ObjectSpacePosition, (_Clamp_7ef923a3f14948b4941a8510ef26c2ea_Out_3.xxx), _Multiply_ff1b071c34424b1a9e402ba831fcdf64_Out_2);
            float3 _Branch_4ecfd0be62ab47738166d09208a68fa4_Out_3;
            Unity_Branch_float3(_Property_5b7413479f844f1cb7bea80b77a3206c_Out_0, _Multiply_ff1b071c34424b1a9e402ba831fcdf64_Out_2, IN.ObjectSpacePosition, _Branch_4ecfd0be62ab47738166d09208a68fa4_Out_3);
            description.Position = _Branch_4ecfd0be62ab47738166d09208a68fa4_Out_3;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float2 _Property_d52ce9ecd75b4bad89952518448a18bd_Out_0 = Vector2_b8d33165554f47859715f268ca93d9dd;
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_R_1 = _Property_d52ce9ecd75b4bad89952518448a18bd_Out_0[0];
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_G_2 = _Property_d52ce9ecd75b4bad89952518448a18bd_Out_0[1];
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_B_3 = 0;
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_A_4 = 0;
            float2 _Vector2_5caa6ab780d54dd099c9b36d34618d6f_Out_0 = float2(0, _Split_f615a84a71594c0fbccf5f1cd8ff52d2_G_2);
            float _Property_bfd4fac952864b44b810378fc1d799d2_Out_0 = _Speed;
            float2 _Vector2_9f3dd73887a641b9ac56504bf68ae5ba_Out_0 = float2(0, _Property_bfd4fac952864b44b810378fc1d799d2_Out_0);
            float _Property_218ab6af02df441aa7e739c2142ea8f6_Out_0 = _UnscaledTime;
            float _Add_e624e733158949be9b27794d1bb13482_Out_2;
            Unity_Add_float(IN.TimeParameters.x, _Property_218ab6af02df441aa7e739c2142ea8f6_Out_0, _Add_e624e733158949be9b27794d1bb13482_Out_2);
            float2 _Multiply_f8718557c7ca47b8b3fc39d36d09ed13_Out_2;
            Unity_Multiply_float2_float2(_Vector2_9f3dd73887a641b9ac56504bf68ae5ba_Out_0, (_Add_e624e733158949be9b27794d1bb13482_Out_2.xx), _Multiply_f8718557c7ca47b8b3fc39d36d09ed13_Out_2);
            float2 _TilingAndOffset_b0546cddf2a148a1a52847c96ea74e8b_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Vector2_5caa6ab780d54dd099c9b36d34618d6f_Out_0, _Multiply_f8718557c7ca47b8b3fc39d36d09ed13_Out_2, _TilingAndOffset_b0546cddf2a148a1a52847c96ea74e8b_Out_3);
            float _GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2;
            Unity_GradientNoise_float(_TilingAndOffset_b0546cddf2a148a1a52847c96ea74e8b_Out_3, 10, _GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2);
            float _InvertColors_18445188fb7e4b59a2f8eab3c047837c_Out_1;
            float _InvertColors_18445188fb7e4b59a2f8eab3c047837c_InvertColors = float (1);
            Unity_InvertColors_float(_GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2, _InvertColors_18445188fb7e4b59a2f8eab3c047837c_InvertColors, _InvertColors_18445188fb7e4b59a2f8eab3c047837c_Out_1);
            float _Property_a57dcd5331a1400ba6f1c8c70db49e2f_Out_0 = _IsTransparentTexture;
            UnityTexture2D _Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0 = UnityBuildTexture2DStructNoScale(_TargetTexture2);
            float4 _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0.tex, _Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0.samplerstate, _Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_R_4 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.r;
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_G_5 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.g;
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_B_6 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.b;
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_A_7 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.a;
            float _Split_9648a8090aa14ce0abb7445e21241c43_R_1 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[0];
            float _Split_9648a8090aa14ce0abb7445e21241c43_G_2 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[1];
            float _Split_9648a8090aa14ce0abb7445e21241c43_B_3 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[2];
            float _Split_9648a8090aa14ce0abb7445e21241c43_A_4 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[3];
            float _Add_3d5c77838ac34011b65ca68f063f5dd4_Out_2;
            Unity_Add_float(_Split_9648a8090aa14ce0abb7445e21241c43_A_4, 0, _Add_3d5c77838ac34011b65ca68f063f5dd4_Out_2);
            UnityTexture2D _Property_4ae289f5d240471ca9e6399a761815f7_Out_0 = UnityBuildTexture2DStructNoScale(_TargetTexture);
            float4 _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0 = SAMPLE_TEXTURE2D(_Property_4ae289f5d240471ca9e6399a761815f7_Out_0.tex, _Property_4ae289f5d240471ca9e6399a761815f7_Out_0.samplerstate, _Property_4ae289f5d240471ca9e6399a761815f7_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_R_4 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.r;
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_G_5 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.g;
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_B_6 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.b;
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_A_7 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.a;
            float4 _Branch_88817652b5b749c2b29dd2e4f32d135b_Out_3;
            Unity_Branch_float4(_Property_a57dcd5331a1400ba6f1c8c70db49e2f_Out_0, (_Add_3d5c77838ac34011b65ca68f063f5dd4_Out_2.xxxx), _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0, _Branch_88817652b5b749c2b29dd2e4f32d135b_Out_3);
            float _Property_52a7d19f4bd843458ef1288ac990f460_Out_0 = _Brightness;
            float4 _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2;
            Unity_Multiply_float4_float4(_Branch_88817652b5b749c2b29dd2e4f32d135b_Out_3, (_Property_52a7d19f4bd843458ef1288ac990f460_Out_0.xxxx), _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2);
            float4 _Multiply_420383d8fff643baac82888de6defe00_Out_2;
            Unity_Multiply_float4_float4((_InvertColors_18445188fb7e4b59a2f8eab3c047837c_Out_1.xxxx), _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2, _Multiply_420383d8fff643baac82888de6defe00_Out_2);
            float _Property_489ef5838aaf42f19939756c5a5c1f87_Out_0 = Vector1_cdea67384fbe4b94baa4476968e9f762;
            float _Multiply_4c94c05e6cb444119157b17ebdc161e0_Out_2;
            Unity_Multiply_float_float(_GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2, _Property_489ef5838aaf42f19939756c5a5c1f87_Out_0, _Multiply_4c94c05e6cb444119157b17ebdc161e0_Out_2);
            float4 _Multiply_638ee8b9bb1b4ba28fd703a3fc89ee32_Out_2;
            Unity_Multiply_float4_float4((_Multiply_4c94c05e6cb444119157b17ebdc161e0_Out_2.xxxx), _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2, _Multiply_638ee8b9bb1b4ba28fd703a3fc89ee32_Out_2);
            float4 _Add_d0dacea2b508410a8ff9d4af2127f57f_Out_2;
            Unity_Add_float4(_Multiply_420383d8fff643baac82888de6defe00_Out_2, _Multiply_638ee8b9bb1b4ba28fd703a3fc89ee32_Out_2, _Add_d0dacea2b508410a8ff9d4af2127f57f_Out_2);
            float4 _Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2;
            Unity_Multiply_float4_float4(_Add_d0dacea2b508410a8ff9d4af2127f57f_Out_2, float4(1, 1, 1, 1), _Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2);
            surface.Alpha = (_Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2).x;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.uv0 =                                        input.uv0;
            output.TimeParameters =                             _TimeParameters.xyz;
        
            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 =                                        input.texCoord0;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
            ENDHLSL
        }
        Pass
        {
            Name "Sprite Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
        
            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest [unity_GUIZTestMode]
        ZWrite Off

        Stencil
        {
             Ref [_Stencil]
             Comp [_StencilComp]
             Pass [_StencilOp]
             ReadMask [_StencilReadMask]
             WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>
        
            // Keywords
            #pragma multi_compile_fragment _ DEBUG_DISPLAY
            // GraphKeywords: <None>
        
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITEFORWARD
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
            // Includes
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float4 interp2 : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyzw =  input.texCoord0;
            output.interp2.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.color = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TargetTexture_TexelSize;
        float4 _TargetTexture2_TexelSize;
        float4 _MainColor;
        float _Intensity;
        float Vector1_cdea67384fbe4b94baa4476968e9f762;
        float2 Vector2_b8d33165554f47859715f268ca93d9dd;
        float _Speed;
        float _Brightness;
        float _Disturbance;
        float4 _MainTex_TexelSize;
        float _UnscaledTime;
        float _Rotation;
        float _IsTransparentTexture;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TargetTexture);
        SAMPLER(sampler_TargetTexture);
        TEXTURE2D(_TargetTexture2);
        SAMPLER(sampler_TargetTexture2);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
            // Graph Includes
            // GraphIncludes: <None>
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Graph Functions
            
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        
        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        {
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Branch_float3(float Predicate, float3 True, float3 False, out float3 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Property_5b7413479f844f1cb7bea80b77a3206c_Out_0 = _Disturbance;
            float2 _Vector2_56b07c3b01d74c2fb762b3665ba1834b_Out_0 = float2(0, 5);
            float2 _Multiply_0f3ce384361c4e13910378af5e11663f_Out_2;
            Unity_Multiply_float2_float2(_Vector2_56b07c3b01d74c2fb762b3665ba1834b_Out_0, (IN.TimeParameters.x.xx), _Multiply_0f3ce384361c4e13910378af5e11663f_Out_2);
            float2 _TilingAndOffset_a2e8ba11d3fd46edb1150ebff07cc875_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (0.01, 1.4), _Multiply_0f3ce384361c4e13910378af5e11663f_Out_2, _TilingAndOffset_a2e8ba11d3fd46edb1150ebff07cc875_Out_3);
            float _GradientNoise_0ee7d9f541974d788d323c1bde3e7e00_Out_2;
            Unity_GradientNoise_float(_TilingAndOffset_a2e8ba11d3fd46edb1150ebff07cc875_Out_3, 10, _GradientNoise_0ee7d9f541974d788d323c1bde3e7e00_Out_2);
            float _InvertColors_820c98bdc41044df961a0881737b2567_Out_1;
            float _InvertColors_820c98bdc41044df961a0881737b2567_InvertColors = float (1);
            Unity_InvertColors_float(_GradientNoise_0ee7d9f541974d788d323c1bde3e7e00_Out_2, _InvertColors_820c98bdc41044df961a0881737b2567_InvertColors, _InvertColors_820c98bdc41044df961a0881737b2567_Out_1);
            float _Remap_933037f6624f4538b1d07f8c3a76a5ed_Out_3;
            Unity_Remap_float(_InvertColors_820c98bdc41044df961a0881737b2567_Out_1, float2 (-1, 1), float2 (0, 1), _Remap_933037f6624f4538b1d07f8c3a76a5ed_Out_3);
            float _Add_fe38576d18f64ce0b764c66710b47170_Out_2;
            Unity_Add_float(_Remap_933037f6624f4538b1d07f8c3a76a5ed_Out_3, 0.35, _Add_fe38576d18f64ce0b764c66710b47170_Out_2);
            float _Clamp_7ef923a3f14948b4941a8510ef26c2ea_Out_3;
            Unity_Clamp_float(_Add_fe38576d18f64ce0b764c66710b47170_Out_2, 0.98, 1, _Clamp_7ef923a3f14948b4941a8510ef26c2ea_Out_3);
            float3 _Multiply_ff1b071c34424b1a9e402ba831fcdf64_Out_2;
            Unity_Multiply_float3_float3(IN.ObjectSpacePosition, (_Clamp_7ef923a3f14948b4941a8510ef26c2ea_Out_3.xxx), _Multiply_ff1b071c34424b1a9e402ba831fcdf64_Out_2);
            float3 _Branch_4ecfd0be62ab47738166d09208a68fa4_Out_3;
            Unity_Branch_float3(_Property_5b7413479f844f1cb7bea80b77a3206c_Out_0, _Multiply_ff1b071c34424b1a9e402ba831fcdf64_Out_2, IN.ObjectSpacePosition, _Branch_4ecfd0be62ab47738166d09208a68fa4_Out_3);
            description.Position = _Branch_4ecfd0be62ab47738166d09208a68fa4_Out_3;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float2 _Property_d52ce9ecd75b4bad89952518448a18bd_Out_0 = Vector2_b8d33165554f47859715f268ca93d9dd;
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_R_1 = _Property_d52ce9ecd75b4bad89952518448a18bd_Out_0[0];
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_G_2 = _Property_d52ce9ecd75b4bad89952518448a18bd_Out_0[1];
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_B_3 = 0;
            float _Split_f615a84a71594c0fbccf5f1cd8ff52d2_A_4 = 0;
            float2 _Vector2_5caa6ab780d54dd099c9b36d34618d6f_Out_0 = float2(0, _Split_f615a84a71594c0fbccf5f1cd8ff52d2_G_2);
            float _Property_bfd4fac952864b44b810378fc1d799d2_Out_0 = _Speed;
            float2 _Vector2_9f3dd73887a641b9ac56504bf68ae5ba_Out_0 = float2(0, _Property_bfd4fac952864b44b810378fc1d799d2_Out_0);
            float _Property_218ab6af02df441aa7e739c2142ea8f6_Out_0 = _UnscaledTime;
            float _Add_e624e733158949be9b27794d1bb13482_Out_2;
            Unity_Add_float(IN.TimeParameters.x, _Property_218ab6af02df441aa7e739c2142ea8f6_Out_0, _Add_e624e733158949be9b27794d1bb13482_Out_2);
            float2 _Multiply_f8718557c7ca47b8b3fc39d36d09ed13_Out_2;
            Unity_Multiply_float2_float2(_Vector2_9f3dd73887a641b9ac56504bf68ae5ba_Out_0, (_Add_e624e733158949be9b27794d1bb13482_Out_2.xx), _Multiply_f8718557c7ca47b8b3fc39d36d09ed13_Out_2);
            float2 _TilingAndOffset_b0546cddf2a148a1a52847c96ea74e8b_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Vector2_5caa6ab780d54dd099c9b36d34618d6f_Out_0, _Multiply_f8718557c7ca47b8b3fc39d36d09ed13_Out_2, _TilingAndOffset_b0546cddf2a148a1a52847c96ea74e8b_Out_3);
            float _GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2;
            Unity_GradientNoise_float(_TilingAndOffset_b0546cddf2a148a1a52847c96ea74e8b_Out_3, 10, _GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2);
            float _InvertColors_18445188fb7e4b59a2f8eab3c047837c_Out_1;
            float _InvertColors_18445188fb7e4b59a2f8eab3c047837c_InvertColors = float (1);
            Unity_InvertColors_float(_GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2, _InvertColors_18445188fb7e4b59a2f8eab3c047837c_InvertColors, _InvertColors_18445188fb7e4b59a2f8eab3c047837c_Out_1);
            float _Property_a57dcd5331a1400ba6f1c8c70db49e2f_Out_0 = _IsTransparentTexture;
            UnityTexture2D _Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0 = UnityBuildTexture2DStructNoScale(_TargetTexture2);
            float4 _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0.tex, _Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0.samplerstate, _Property_7db63a1801e84ba8bda9cfc4d4cc6fff_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_R_4 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.r;
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_G_5 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.g;
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_B_6 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.b;
            float _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_A_7 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0.a;
            float _Split_9648a8090aa14ce0abb7445e21241c43_R_1 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[0];
            float _Split_9648a8090aa14ce0abb7445e21241c43_G_2 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[1];
            float _Split_9648a8090aa14ce0abb7445e21241c43_B_3 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[2];
            float _Split_9648a8090aa14ce0abb7445e21241c43_A_4 = _SampleTexture2D_ca30950c4ea54cdda7b52b826ed5c07d_RGBA_0[3];
            float _Add_3d5c77838ac34011b65ca68f063f5dd4_Out_2;
            Unity_Add_float(_Split_9648a8090aa14ce0abb7445e21241c43_A_4, 0, _Add_3d5c77838ac34011b65ca68f063f5dd4_Out_2);
            UnityTexture2D _Property_4ae289f5d240471ca9e6399a761815f7_Out_0 = UnityBuildTexture2DStructNoScale(_TargetTexture);
            float4 _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0 = SAMPLE_TEXTURE2D(_Property_4ae289f5d240471ca9e6399a761815f7_Out_0.tex, _Property_4ae289f5d240471ca9e6399a761815f7_Out_0.samplerstate, _Property_4ae289f5d240471ca9e6399a761815f7_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_R_4 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.r;
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_G_5 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.g;
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_B_6 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.b;
            float _SampleTexture2D_f339278fbf584acc9559e45f599792c6_A_7 = _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0.a;
            float4 _Branch_88817652b5b749c2b29dd2e4f32d135b_Out_3;
            Unity_Branch_float4(_Property_a57dcd5331a1400ba6f1c8c70db49e2f_Out_0, (_Add_3d5c77838ac34011b65ca68f063f5dd4_Out_2.xxxx), _SampleTexture2D_f339278fbf584acc9559e45f599792c6_RGBA_0, _Branch_88817652b5b749c2b29dd2e4f32d135b_Out_3);
            float _Property_52a7d19f4bd843458ef1288ac990f460_Out_0 = _Brightness;
            float4 _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2;
            Unity_Multiply_float4_float4(_Branch_88817652b5b749c2b29dd2e4f32d135b_Out_3, (_Property_52a7d19f4bd843458ef1288ac990f460_Out_0.xxxx), _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2);
            float4 _Multiply_420383d8fff643baac82888de6defe00_Out_2;
            Unity_Multiply_float4_float4((_InvertColors_18445188fb7e4b59a2f8eab3c047837c_Out_1.xxxx), _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2, _Multiply_420383d8fff643baac82888de6defe00_Out_2);
            float _Property_489ef5838aaf42f19939756c5a5c1f87_Out_0 = Vector1_cdea67384fbe4b94baa4476968e9f762;
            float _Multiply_4c94c05e6cb444119157b17ebdc161e0_Out_2;
            Unity_Multiply_float_float(_GradientNoise_de67ed159b9247b4beb64735ee64c9f4_Out_2, _Property_489ef5838aaf42f19939756c5a5c1f87_Out_0, _Multiply_4c94c05e6cb444119157b17ebdc161e0_Out_2);
            float4 _Multiply_638ee8b9bb1b4ba28fd703a3fc89ee32_Out_2;
            Unity_Multiply_float4_float4((_Multiply_4c94c05e6cb444119157b17ebdc161e0_Out_2.xxxx), _Multiply_e976adc19b4344c89140fd39260ad0ea_Out_2, _Multiply_638ee8b9bb1b4ba28fd703a3fc89ee32_Out_2);
            float4 _Add_d0dacea2b508410a8ff9d4af2127f57f_Out_2;
            Unity_Add_float4(_Multiply_420383d8fff643baac82888de6defe00_Out_2, _Multiply_638ee8b9bb1b4ba28fd703a3fc89ee32_Out_2, _Add_d0dacea2b508410a8ff9d4af2127f57f_Out_2);
            float4 _Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2;
            Unity_Multiply_float4_float4(_Add_d0dacea2b508410a8ff9d4af2127f57f_Out_2, float4(1, 1, 1, 1), _Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2);
            float4 _Property_99171730b95a4aa7b036e0b6a914c3b0_Out_0 = IsGammaSpace() ? LinearToSRGB(_MainColor) : _MainColor;
            float4 _Multiply_64d3b80fdaec4e859cdf686d4db3b57a_Out_2;
            Unity_Multiply_float4_float4(_Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2, _Property_99171730b95a4aa7b036e0b6a914c3b0_Out_0, _Multiply_64d3b80fdaec4e859cdf686d4db3b57a_Out_2);
            float _Property_1900347b72814f478881370b66df9ba3_Out_0 = _Intensity;
            float4 _Multiply_d562341f791c4c0ebc86e80540330ecd_Out_2;
            Unity_Multiply_float4_float4(_Multiply_64d3b80fdaec4e859cdf686d4db3b57a_Out_2, (_Property_1900347b72814f478881370b66df9ba3_Out_0.xxxx), _Multiply_d562341f791c4c0ebc86e80540330ecd_Out_2);
            surface.BaseColor = (_Multiply_d562341f791c4c0ebc86e80540330ecd_Out_2.xyz);
            surface.Alpha = (_Multiply_a08cc529f47d453891e48bf9e6b01c17_Out_2).x;
            surface.NormalTS = IN.TangentSpaceNormal;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.uv0 =                                        input.uv0;
            output.TimeParameters =                             _TimeParameters.xyz;
        
            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
            output.TangentSpaceNormal =                         float3(0.0f, 0.0f, 1.0f);
        
        
            output.uv0 =                                        input.texCoord0;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteForwardPass.hlsl"
        
            ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}
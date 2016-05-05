//Version=1
Shader"UNOShader/_Library/Helpers/Xray"
{
	Properties
	{
		_XrayTex ("Xray Texture", 2D) = "white" {}		
		_XrayColor ("Xray Color", Color) = (1,1,1,1)
		_XrayIntensity ("Xray Intensity", Range (1, 10)) = 1
	}
	SubShader
	{
		Tags
		{
			//--- "RenderType" sets the group that it belongs to type and uses: Opaque, Transparent,
			//--- TransparentCutout, Background, Overlay(Gui,halo,Flare shaders), TreeOpaque, TreeTransparentCutout, TreeBilboard,Grass, GrassBilboard.
			//--- "Queue" sets order and uses: Background (for skyboxes), Geometry(default), AlphaTest(?, water),
			//--- Transparent(draws after AlphaTest, back to front order), Overlay(effects,ie lens flares)
			//--- adding +number to tags "Geometry +1" will affect draw order. B=1000 G=2000 AT= 2450 T=3000 O=4000
			
//			"RenderType" = "Opaque"
//			"Queue" = "Geometry"
//			
//			"RenderType" = "Transparent"
//			"Queue" = "Transparent"
		//	"LightMode" = "ForwardBase"
		}
		Pass
		{
            Name "BASE"
            Tags
			{
			//--- "RenderType" sets the group that it belongs to type and uses: Opaque, Transparent,
			//--- TransparentCutout, Background, Overlay(Gui,halo,Flare shaders), TreeOpaque, TreeTransparentCutout, TreeBilboard,Grass, GrassBilboard.
			//--- "Queue" sets order and uses: Background (for skyboxes), Geometry(default), AlphaTest(?, water),
			//--- Transparent(draws after AlphaTest, back to front order), Overlay(effects,ie lens flares)
			//--- adding +number to tags "Geometry +1" will affect draw order. B=1000 G=2000 AT= 2450 T=3000 O=4000
//			"RenderType" = "Transparent"
//			"Queue" = "Transparent"
			//"LightMode" = "ForwardBase"
			}
            Cull Front           	           	 
           	Blend SrcAlpha OneMinusSrcAlpha //-- transparency enable
           	Zwrite Off
           	Ztest Always
           	           
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag            
 											
 			fixed4 _XrayColor;
 			fixed _XrayIntensity;
 			 		 		 		
           	struct customData
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
            };
           
            v2f vert(customData v)
            {
                v2f o;
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);                
                o.color = fixed4(_XrayColor.rgb * _XrayIntensity,_XrayColor.a);
                return o;
            }
           
            half4 frag(v2f i) :COLOR
            {          
            	fixed4 result = fixed4(0,0,0,0);  	            	
        		result = i.color;
                return result;				
            }
                   
            ENDCG
        }// ------------------- Pass ---------------------------------------				
	} //-------------------------------SubShader-------------------------------
	//Fallback "VertexLit" // for shadows
}

void AdditionalLights_float(half3 SpecColor, half Smoothness, half3 WorldPosition, half3 WorldNormal, half3 WorldView, out half3 Diffuse, out half3 Specular)
{
   half3 diffuseColor = 0;
   half3 specularColor = 0;

#ifndef SHADERGRAPH_PREVIEW
   Smoothness = exp2(10 * Smoothness + 1);
   WorldNormal = normalize(WorldNormal);
   WorldView = SafeNormalize(WorldView);
   int pixelLightCount = GetAdditionalLightsCount();
   for (int i = 0; i < pixelLightCount; ++i)
   {
       Light light = GetAdditionalLight(i, WorldPosition);
       half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
       diffuseColor += LightingLambert(attenuatedLightColor, light.direction, WorldNormal);
       specularColor += LightingSpecular(attenuatedLightColor, light.direction, WorldNormal, WorldView, half4(SpecColor, 0), Smoothness);
   }
#endif

   Diffuse = diffuseColor;
   Specular = specularColor;
}
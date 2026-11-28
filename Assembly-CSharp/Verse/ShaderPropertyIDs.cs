using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200070B RID: 1803
	[StaticConstructorOnStartup]
	public static class ShaderPropertyIDs
	{
		// Token: 0x04002518 RID: 9496
		private static readonly string PlanetSunLightDirectionName = "_PlanetSunLightDirection";

		// Token: 0x04002519 RID: 9497
		private static readonly string PlanetSunLightEnabledName = "_PlanetSunLightEnabled";

		// Token: 0x0400251A RID: 9498
		private static readonly string PlanetOriginName = "_PlanetOrigin";

		// Token: 0x0400251B RID: 9499
		private static readonly string PlanetRadiusName = "_PlanetRadius";

		// Token: 0x0400251C RID: 9500
		private static readonly string GlowRadiusName = "_GlowRadius";

		// Token: 0x0400251D RID: 9501
		private static readonly string MapSunLightDirectionName = "_CastVect";

		// Token: 0x0400251E RID: 9502
		private static readonly string GameSecondsName = "_GameSeconds";

		// Token: 0x0400251F RID: 9503
		private static readonly string ColorName = "_Color";

		// Token: 0x04002520 RID: 9504
		private static readonly string ColorTwoName = "_ColorTwo";

		// Token: 0x04002521 RID: 9505
		private static readonly string SecondaryTexName = "_SecondaryTex";

		// Token: 0x04002522 RID: 9506
		private static readonly string MaskTexName = "_MaskTex";

		// Token: 0x04002523 RID: 9507
		private static readonly string MacroTexName = "_MacroTex";

		// Token: 0x04002524 RID: 9508
		private static readonly string SwayHeadName = "_SwayHead";

		// Token: 0x04002525 RID: 9509
		private static readonly string ShockwaveSpanName = "_ShockwaveSpan";

		// Token: 0x04002526 RID: 9510
		private static readonly string AgeSecsName = "_AgeSecs";

		// Token: 0x04002527 RID: 9511
		private static readonly string RandomPerObjectName = "_RandomPerObject";

		// Token: 0x04002528 RID: 9512
		private static readonly string RandomPerObjectOffsetRandomName = "_RandomPerObjectOffsetRandom";

		// Token: 0x04002529 RID: 9513
		private static readonly string RotationName = "_Rotation";

		// Token: 0x0400252A RID: 9514
		private static readonly string OverlayOpacityName = "_OverlayOpacity";

		// Token: 0x0400252B RID: 9515
		private static readonly string OverlayColorName = "_OverlayColor";

		// Token: 0x0400252C RID: 9516
		private static readonly string AgeSecsPausableName = "_AgeSecsPausable";

		// Token: 0x0400252D RID: 9517
		private static readonly string MainTextureOffsetName = "_Main_TexOffset";

		// Token: 0x0400252E RID: 9518
		private static readonly string MainTextureScaleName = "_Main_TexScale";

		// Token: 0x0400252F RID: 9519
		private static readonly string MaskTextureOffsetName = "_Mask_TexOffset";

		// Token: 0x04002530 RID: 9520
		private static readonly string MaskTextureScaleName = "_Mask_TexScale";

		// Token: 0x04002531 RID: 9521
		private static readonly string ShockwaveColorName = "_ShockwaveColor";

		// Token: 0x04002532 RID: 9522
		private static readonly string WaterCastVectSunName = "_WaterCastVectSun";

		// Token: 0x04002533 RID: 9523
		private static readonly string WaterCastVectMoonName = "_WaterCastVectMoon";

		// Token: 0x04002534 RID: 9524
		private static readonly string WaterOutputTexName = "_WaterOutputTex";

		// Token: 0x04002535 RID: 9525
		private static readonly string WaterOffsetTexName = "_WaterOffsetTex";

		// Token: 0x04002536 RID: 9526
		private static readonly string WaterReflectionTexName = "_WaterReflectionTex";

		// Token: 0x04002537 RID: 9527
		private static readonly string ShadowCompositeTexName = "_ShadowCompositeTex";

		// Token: 0x04002538 RID: 9528
		private static readonly string FallIntensityName = "_FallIntensity";

		// Token: 0x04002539 RID: 9529
		private static readonly string MainTex_STName = "_MainTex_ST";

		// Token: 0x0400253A RID: 9530
		private static readonly string SquashNStretchName = "_SquashNStretch";

		// Token: 0x0400253B RID: 9531
		private static readonly string WorkingName = "_Working";

		// Token: 0x0400253C RID: 9532
		private static readonly string FadeTexName = "_FadeTex";

		// Token: 0x0400253D RID: 9533
		private static readonly string FadeTexScrollSpeedName = "_FadeTexScrollSpeed";

		// Token: 0x0400253E RID: 9534
		private static readonly string ScrollSpeedName = "_ScrollSpeed";

		// Token: 0x0400253F RID: 9535
		private static readonly string FadeTexScaleName = "_FadeTexScale";

		// Token: 0x04002540 RID: 9536
		private static readonly string PollutedTexName = "_PollutedTex";

		// Token: 0x04002541 RID: 9537
		private static readonly string IsPollutedName = "_IsPolluted";

		// Token: 0x04002542 RID: 9538
		private static readonly string ToxGasTexName = "_ToxGasTex";

		// Token: 0x04002543 RID: 9539
		private static readonly string RotGasTexName = "_RotGasTex";

		// Token: 0x04002544 RID: 9540
		private static readonly string DeadlifeDustTexName = "_DeadlifeDustTex";

		// Token: 0x04002545 RID: 9541
		private static readonly string DestructionStartAgeName = "_DestructionStartAgeSecs";

		// Token: 0x04002546 RID: 9542
		private static readonly string PawnCenterWorldName = "_pawnCenterWorld";

		// Token: 0x04002547 RID: 9543
		private static readonly string PawnDrawSizeWorldName = "_pawnDrawSizeWorld";

		// Token: 0x04002548 RID: 9544
		private static readonly string AlphaAddTexName = "_AlphaAddTex";

		// Token: 0x04002549 RID: 9545
		private static readonly string BurnTexName = "_BurnTex";

		// Token: 0x0400254A RID: 9546
		private static readonly string BurnColorName = "_BurnColor";

		// Token: 0x0400254B RID: 9547
		private static readonly string BurnScaleName = "_BurnScale";

		// Token: 0x0400254C RID: 9548
		private static readonly string PollutionTintColorName = "_PollutionTintColor";

		// Token: 0x0400254D RID: 9549
		private static readonly string MapSizeName = "_MapSize";

		// Token: 0x0400254E RID: 9550
		private static readonly string NoiseTexName = "_NoiseTex";

		// Token: 0x0400254F RID: 9551
		private static readonly string SunsetColorRampName = "_SunsetColorRamp";

		// Token: 0x04002550 RID: 9552
		private static readonly string CloudMapName = "_CloudTex";

		// Token: 0x04002551 RID: 9553
		private static readonly string CloudShaderOpacityName = "_FinalOpacityMult";

		// Token: 0x04002552 RID: 9554
		private static readonly string GameTimeName = "_GameTime";

		// Token: 0x04002553 RID: 9555
		private static readonly string BackgroundModeEnabledName = "_BackgroundModeEnabled";

		// Token: 0x04002554 RID: 9556
		private static readonly string ProgressName = "_Progress";

		// Token: 0x04002555 RID: 9557
		private static readonly string DrawPosName = "_DrawPos";

		// Token: 0x04002556 RID: 9558
		private static readonly string SeedName = "_Seed";

		// Token: 0x04002557 RID: 9559
		private static readonly string MainCameraScreenParamsName = "_MainCameraScreenParams";

		// Token: 0x04002558 RID: 9560
		private static readonly string MainCameraVPName = "_MainCameraVP";

		// Token: 0x04002559 RID: 9561
		public static readonly int PlanetSunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.PlanetSunLightDirectionName);

		// Token: 0x0400255A RID: 9562
		public static readonly int PlanetSunLightEnabled = Shader.PropertyToID(ShaderPropertyIDs.PlanetSunLightEnabledName);

		// Token: 0x0400255B RID: 9563
		public static readonly int PlanetOrigin = Shader.PropertyToID(ShaderPropertyIDs.PlanetOriginName);

		// Token: 0x0400255C RID: 9564
		public static readonly int PlanetRadius = Shader.PropertyToID(ShaderPropertyIDs.PlanetRadiusName);

		// Token: 0x0400255D RID: 9565
		public static readonly int MapSunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.MapSunLightDirectionName);

		// Token: 0x0400255E RID: 9566
		public static readonly int GlowRadius = Shader.PropertyToID(ShaderPropertyIDs.GlowRadiusName);

		// Token: 0x0400255F RID: 9567
		public static readonly int GameSeconds = Shader.PropertyToID(ShaderPropertyIDs.GameSecondsName);

		// Token: 0x04002560 RID: 9568
		public static readonly int AgeSecs = Shader.PropertyToID(ShaderPropertyIDs.AgeSecsName);

		// Token: 0x04002561 RID: 9569
		public static readonly int AgeSecsPausable = Shader.PropertyToID(ShaderPropertyIDs.AgeSecsPausableName);

		// Token: 0x04002562 RID: 9570
		public static readonly int RandomPerObject = Shader.PropertyToID(ShaderPropertyIDs.RandomPerObjectName);

		// Token: 0x04002563 RID: 9571
		public static readonly int RandomPerObjectOffsetRandom = Shader.PropertyToID(ShaderPropertyIDs.RandomPerObjectOffsetRandomName);

		// Token: 0x04002564 RID: 9572
		public static readonly int Rotation = Shader.PropertyToID(ShaderPropertyIDs.RotationName);

		// Token: 0x04002565 RID: 9573
		public static readonly int OverlayOpacity = Shader.PropertyToID(ShaderPropertyIDs.OverlayOpacityName);

		// Token: 0x04002566 RID: 9574
		public static readonly int OverlayColor = Shader.PropertyToID(ShaderPropertyIDs.OverlayColorName);

		// Token: 0x04002567 RID: 9575
		public static readonly int Color = Shader.PropertyToID(ShaderPropertyIDs.ColorName);

		// Token: 0x04002568 RID: 9576
		public static readonly int ColorTwo = Shader.PropertyToID(ShaderPropertyIDs.ColorTwoName);

		// Token: 0x04002569 RID: 9577
		public static readonly int SecondaryTex = Shader.PropertyToID(ShaderPropertyIDs.SecondaryTexName);

		// Token: 0x0400256A RID: 9578
		public static readonly int MaskTex = Shader.PropertyToID(ShaderPropertyIDs.MaskTexName);

		// Token: 0x0400256B RID: 9579
		public static readonly int MacroTex = Shader.PropertyToID(ShaderPropertyIDs.MacroTexName);

		// Token: 0x0400256C RID: 9580
		public static readonly int SwayHead = Shader.PropertyToID(ShaderPropertyIDs.SwayHeadName);

		// Token: 0x0400256D RID: 9581
		public static readonly int ShockwaveColor = Shader.PropertyToID(ShaderPropertyIDs.ShockwaveColorName);

		// Token: 0x0400256E RID: 9582
		public static readonly int ShockwaveSpan = Shader.PropertyToID(ShaderPropertyIDs.ShockwaveSpanName);

		// Token: 0x0400256F RID: 9583
		public static readonly int WaterCastVectSun = Shader.PropertyToID(ShaderPropertyIDs.WaterCastVectSunName);

		// Token: 0x04002570 RID: 9584
		public static readonly int WaterCastVectMoon = Shader.PropertyToID(ShaderPropertyIDs.WaterCastVectMoonName);

		// Token: 0x04002571 RID: 9585
		public static readonly int WaterOutputTex = Shader.PropertyToID(ShaderPropertyIDs.WaterOutputTexName);

		// Token: 0x04002572 RID: 9586
		public static readonly int WaterOffsetTex = Shader.PropertyToID(ShaderPropertyIDs.WaterOffsetTexName);

		// Token: 0x04002573 RID: 9587
		public static readonly int WaterReflectionTex = Shader.PropertyToID(ShaderPropertyIDs.WaterReflectionTexName);

		// Token: 0x04002574 RID: 9588
		public static readonly int ShadowCompositeTex = Shader.PropertyToID(ShaderPropertyIDs.ShadowCompositeTexName);

		// Token: 0x04002575 RID: 9589
		public static readonly int FallIntensity = Shader.PropertyToID(ShaderPropertyIDs.FallIntensityName);

		// Token: 0x04002576 RID: 9590
		public static readonly int MainTextureOffset = Shader.PropertyToID(ShaderPropertyIDs.MainTextureOffsetName);

		// Token: 0x04002577 RID: 9591
		public static readonly int MainTextureScale = Shader.PropertyToID(ShaderPropertyIDs.MainTextureScaleName);

		// Token: 0x04002578 RID: 9592
		public static readonly int MaskTextureOffset = Shader.PropertyToID(ShaderPropertyIDs.MaskTextureOffsetName);

		// Token: 0x04002579 RID: 9593
		public static readonly int MaskTextureScale = Shader.PropertyToID(ShaderPropertyIDs.MaskTextureScaleName);

		// Token: 0x0400257A RID: 9594
		public static readonly int Tiling = Shader.PropertyToID(ShaderPropertyIDs.MainTex_STName);

		// Token: 0x0400257B RID: 9595
		public static readonly int SquashNStretch = Shader.PropertyToID(ShaderPropertyIDs.SquashNStretchName);

		// Token: 0x0400257C RID: 9596
		public static readonly int Working = Shader.PropertyToID(ShaderPropertyIDs.WorkingName);

		// Token: 0x0400257D RID: 9597
		public static readonly int FadeTex = Shader.PropertyToID(ShaderPropertyIDs.FadeTexName);

		// Token: 0x0400257E RID: 9598
		public static readonly int TexScrollSpeed = Shader.PropertyToID(ShaderPropertyIDs.FadeTexScrollSpeedName);

		// Token: 0x0400257F RID: 9599
		public static readonly int ScrollSpeed = Shader.PropertyToID(ShaderPropertyIDs.ScrollSpeedName);

		// Token: 0x04002580 RID: 9600
		public static readonly int TexScale = Shader.PropertyToID(ShaderPropertyIDs.FadeTexScaleName);

		// Token: 0x04002581 RID: 9601
		public static readonly int PollutedTex = Shader.PropertyToID(ShaderPropertyIDs.PollutedTexName);

		// Token: 0x04002582 RID: 9602
		public static readonly int IsPolluted = Shader.PropertyToID(ShaderPropertyIDs.IsPollutedName);

		// Token: 0x04002583 RID: 9603
		public static readonly int ToxGasTex = Shader.PropertyToID(ShaderPropertyIDs.ToxGasTexName);

		// Token: 0x04002584 RID: 9604
		public static readonly int RotGasTex = Shader.PropertyToID(ShaderPropertyIDs.RotGasTexName);

		// Token: 0x04002585 RID: 9605
		public static readonly int DeadlifeDustTex = Shader.PropertyToID(ShaderPropertyIDs.DeadlifeDustTexName);

		// Token: 0x04002586 RID: 9606
		public static readonly int VoidNodeDestructionStartAge = Shader.PropertyToID(ShaderPropertyIDs.DestructionStartAgeName);

		// Token: 0x04002587 RID: 9607
		public static readonly int PawnCenterWorld = Shader.PropertyToID(ShaderPropertyIDs.PawnCenterWorldName);

		// Token: 0x04002588 RID: 9608
		public static readonly int PawnDrawSizeWorld = Shader.PropertyToID(ShaderPropertyIDs.PawnDrawSizeWorldName);

		// Token: 0x04002589 RID: 9609
		public static readonly int AlphaAddTex = Shader.PropertyToID(ShaderPropertyIDs.AlphaAddTexName);

		// Token: 0x0400258A RID: 9610
		public static readonly int BurnTex = Shader.PropertyToID(ShaderPropertyIDs.BurnTexName);

		// Token: 0x0400258B RID: 9611
		public static readonly int BurnColor = Shader.PropertyToID(ShaderPropertyIDs.BurnColorName);

		// Token: 0x0400258C RID: 9612
		public static readonly int BurnScale = Shader.PropertyToID(ShaderPropertyIDs.BurnScaleName);

		// Token: 0x0400258D RID: 9613
		public static readonly int PollutionTintColor = Shader.PropertyToID(ShaderPropertyIDs.PollutionTintColorName);

		// Token: 0x0400258E RID: 9614
		public static readonly int MapSize = Shader.PropertyToID(ShaderPropertyIDs.MapSizeName);

		// Token: 0x0400258F RID: 9615
		public static readonly int NoiseTex = Shader.PropertyToID(ShaderPropertyIDs.NoiseTexName);

		// Token: 0x04002590 RID: 9616
		public static readonly int SunsetColorRamp = Shader.PropertyToID(ShaderPropertyIDs.SunsetColorRampName);

		// Token: 0x04002591 RID: 9617
		public static readonly int CloudMap = Shader.PropertyToID(ShaderPropertyIDs.CloudMapName);

		// Token: 0x04002592 RID: 9618
		public static readonly int CloudShaderOpacity = Shader.PropertyToID(ShaderPropertyIDs.CloudShaderOpacityName);

		// Token: 0x04002593 RID: 9619
		public static readonly int GameTime = Shader.PropertyToID(ShaderPropertyIDs.GameTimeName);

		// Token: 0x04002594 RID: 9620
		public static readonly int BackgroundModeEnabled = Shader.PropertyToID(ShaderPropertyIDs.BackgroundModeEnabledName);

		// Token: 0x04002595 RID: 9621
		public static readonly int Progress = Shader.PropertyToID(ShaderPropertyIDs.ProgressName);

		// Token: 0x04002596 RID: 9622
		public static readonly int DrawPos = Shader.PropertyToID(ShaderPropertyIDs.DrawPosName);

		// Token: 0x04002597 RID: 9623
		public static readonly int Seed = Shader.PropertyToID(ShaderPropertyIDs.SeedName);

		// Token: 0x04002598 RID: 9624
		public static readonly int MainCameraScreenParams = Shader.PropertyToID(ShaderPropertyIDs.MainCameraScreenParamsName);

		// Token: 0x04002599 RID: 9625
		public static readonly int MainCameraVP = Shader.PropertyToID(ShaderPropertyIDs.MainCameraVPName);
	}
}

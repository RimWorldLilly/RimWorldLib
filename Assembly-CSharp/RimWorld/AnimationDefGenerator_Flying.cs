using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E96 RID: 3734
	public static class AnimationDefGenerator_Flying
	{
		// Token: 0x06006278 RID: 25208 RVA: 0x002360F0 File Offset: 0x002342F0
		public static void InitializeNeededDefs(bool hotReload = false)
		{
			foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
			{
				if (!pawnKindDef.flyingAnimationFramePathPrefix.NullOrEmpty())
				{
					bool flag = !pawnKindDef.flyingAnimationFramePathPrefixFemale.NullOrEmpty();
					List<string> list = new List<string>();
					List<string> list2 = new List<string>();
					List<string> list3 = new List<string>();
					List<string> list4 = flag ? new List<string>() : list;
					List<string> list5 = flag ? new List<string>() : list2;
					List<string> list6 = flag ? new List<string>() : list3;
					for (int i = 1; i <= pawnKindDef.flyingAnimationFrameCount; i++)
					{
						list.Add(pawnKindDef.flyingAnimationFramePathPrefix + i.ToString() + "_east");
						list2.Add(pawnKindDef.flyingAnimationFramePathPrefix + i.ToString() + "_north");
						list3.Add(pawnKindDef.flyingAnimationFramePathPrefix + i.ToString() + "_south");
						if (flag)
						{
							list4.Add(pawnKindDef.flyingAnimationFramePathPrefixFemale + i.ToString() + "_east");
							list5.Add(pawnKindDef.flyingAnimationFramePathPrefixFemale + i.ToString() + "_north");
							list6.Add(pawnKindDef.flyingAnimationFramePathPrefixFemale + i.ToString() + "_south");
						}
					}
					bool flyingAnimationInheritColors = pawnKindDef.flyingAnimationInheritColors;
					float drawSize = pawnKindDef.flyingAnimationDrawSize;
					bool flyingAnimationDrawSizeIsMultiplier = pawnKindDef.flyingAnimationDrawSizeIsMultiplier;
					int flyingAnimationTicksPerFrame = pawnKindDef.flyingAnimationTicksPerFrame;
					if (flyingAnimationInheritColors || flag)
					{
						Dictionary<Color, ValueTuple<AnimationDef, AnimationDef, AnimationDef>> dictionary = new Dictionary<Color, ValueTuple<AnimationDef, AnimationDef, AnimationDef>>();
						Dictionary<Color, ValueTuple<AnimationDef, AnimationDef, AnimationDef>> dictionary2 = flag ? new Dictionary<Color, ValueTuple<AnimationDef, AnimationDef, AnimationDef>>() : dictionary;
						using (List<PawnKindLifeStage>.Enumerator enumerator2 = pawnKindDef.lifeStages.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								PawnKindLifeStage pawnKindLifeStage = enumerator2.Current;
								Color color = pawnKindLifeStage.bodyGraphicData.color;
								if (flyingAnimationDrawSizeIsMultiplier)
								{
									drawSize = pawnKindLifeStage.bodyGraphicData.drawSize.x * pawnKindDef.flyingAnimationDrawSize;
								}
								ValueTuple<AnimationDef, AnimationDef, AnimationDef> valueTuple;
								if (dictionary.TryGetValue(color, out valueTuple))
								{
									pawnKindLifeStage.flyingAnimationEast = valueTuple.Item1;
									pawnKindLifeStage.flyingAnimationNorth = valueTuple.Item2;
									pawnKindLifeStage.flyingAnimationSouth = valueTuple.Item3;
								}
								else
								{
									AnimationDef animationDef = AnimationDefGenerator_Flying.CreateAnimFromFrames(list, flyingAnimationTicksPerFrame, drawSize, pawnKindDef.defName + "FlyEast", new Color?(color), hotReload);
									AnimationDef animationDef2 = AnimationDefGenerator_Flying.CreateAnimFromFrames(list2, flyingAnimationTicksPerFrame, drawSize, pawnKindDef.defName + "FlyNorth", new Color?(color), hotReload);
									AnimationDef animationDef3 = AnimationDefGenerator_Flying.CreateAnimFromFrames(list3, flyingAnimationTicksPerFrame, drawSize, pawnKindDef.defName + "FlySouth", new Color?(color), hotReload);
									pawnKindLifeStage.flyingAnimationEast = animationDef;
									pawnKindLifeStage.flyingAnimationNorth = animationDef2;
									pawnKindLifeStage.flyingAnimationSouth = animationDef3;
									dictionary[color] = new ValueTuple<AnimationDef, AnimationDef, AnimationDef>(animationDef, animationDef2, animationDef3);
								}
								if (pawnKindLifeStage.femaleGraphicData != null)
								{
									color = pawnKindLifeStage.femaleGraphicData.color;
									if (flyingAnimationDrawSizeIsMultiplier)
									{
										drawSize = pawnKindLifeStage.femaleGraphicData.drawSize.x * pawnKindDef.flyingAnimationDrawSize;
									}
									if (dictionary2.TryGetValue(color, out valueTuple))
									{
										pawnKindLifeStage.flyingAnimationEastFemale = valueTuple.Item1;
										pawnKindLifeStage.flyingAnimationNorthFemale = valueTuple.Item2;
										pawnKindLifeStage.flyingAnimationSouthFemale = valueTuple.Item3;
									}
									else
									{
										AnimationDef animationDef4 = AnimationDefGenerator_Flying.CreateAnimFromFrames(list4, flyingAnimationTicksPerFrame, drawSize, pawnKindDef.defName + "FlyEastFemale", new Color?(color), hotReload);
										AnimationDef animationDef5 = AnimationDefGenerator_Flying.CreateAnimFromFrames(list5, flyingAnimationTicksPerFrame, drawSize, pawnKindDef.defName + "FlyNorthFemale", new Color?(color), hotReload);
										AnimationDef animationDef6 = AnimationDefGenerator_Flying.CreateAnimFromFrames(list6, flyingAnimationTicksPerFrame, drawSize, pawnKindDef.defName + "FlySouthFemale", new Color?(color), hotReload);
										pawnKindLifeStage.flyingAnimationEastFemale = animationDef4;
										pawnKindLifeStage.flyingAnimationNorthFemale = animationDef5;
										pawnKindLifeStage.flyingAnimationSouthFemale = animationDef6;
										dictionary2[color] = new ValueTuple<AnimationDef, AnimationDef, AnimationDef>(animationDef4, animationDef5, animationDef6);
									}
								}
							}
							continue;
						}
					}
					if (flyingAnimationDrawSizeIsMultiplier)
					{
						drawSize = pawnKindDef.lifeStages.First<PawnKindLifeStage>().bodyGraphicData.drawSize.x * pawnKindDef.flyingAnimationDrawSize;
					}
					AnimationDef flyingAnimationEast = AnimationDefGenerator_Flying.CreateAnimFromFrames(list, flyingAnimationTicksPerFrame, drawSize, pawnKindDef.defName + "FlyEast", null, hotReload);
					AnimationDef flyingAnimationNorth = AnimationDefGenerator_Flying.CreateAnimFromFrames(list2, flyingAnimationTicksPerFrame, drawSize, pawnKindDef.defName + "FlyNorth", null, hotReload);
					AnimationDef flyingAnimationSouth = AnimationDefGenerator_Flying.CreateAnimFromFrames(list3, flyingAnimationTicksPerFrame, drawSize, pawnKindDef.defName + "FlySouth", null, hotReload);
					foreach (PawnKindLifeStage pawnKindLifeStage2 in pawnKindDef.lifeStages)
					{
						pawnKindLifeStage2.flyingAnimationEast = flyingAnimationEast;
						pawnKindLifeStage2.flyingAnimationNorth = flyingAnimationNorth;
						pawnKindLifeStage2.flyingAnimationSouth = flyingAnimationSouth;
					}
				}
			}
		}

		// Token: 0x06006279 RID: 25209 RVA: 0x00236620 File Offset: 0x00234820
		public static IEnumerable<GraphicStateDef> ImpliedGraphicStateDefs()
		{
			return new AnimationDefGenerator_Flying.<ImpliedGraphicStateDefs>d__3(-2);
		}

		// Token: 0x0600627A RID: 25210 RVA: 0x00236629 File Offset: 0x00234829
		public static IEnumerable<AnimationDef> ImpliedAnimationDefs()
		{
			return new AnimationDefGenerator_Flying.<ImpliedAnimationDefs>d__4(-2);
		}

		// Token: 0x0600627B RID: 25211 RVA: 0x00236634 File Offset: 0x00234834
		private static AnimationDef CreateAnimFromFrames(List<string> framePaths, int ticksPerFrame, float drawSize, string nameRoot, Color? tint = null, bool hotReload = false)
		{
			string text;
			if (tint == null)
			{
				text = nameRoot;
			}
			else
			{
				Color32 color = tint.Value;
				int value = (int)color.a << 24 | (int)color.r << 16 | (int)color.g << 8 | (int)color.b;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted(nameRoot);
				defaultInterpolatedStringHandler.AppendLiteral("_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(value, "x8");
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			AnimationDef animationDef = hotReload ? (DefDatabase<AnimationDef>.GetNamed(text, false) ?? new AnimationDef()) : new AnimationDef();
			animationDef.durationTicks = (framePaths.Count + 1) * ticksPerFrame;
			animationDef.defName = text;
			animationDef.startOnRandomTick = false;
			animationDef.keyframeParts = new Dictionary<PawnRenderNodeTagDef, KeyframeAnimationPart>();
			PawnRenderNodeTagDef named = DefDatabase<PawnRenderNodeTagDef>.GetNamed("Body", true);
			KeyframeAnimationPart keyframeAnimationPart = animationDef.keyframeParts[named] = new KeyframeAnimationPart();
			keyframeAnimationPart.workerType = typeof(AnimationWorker_Keyframes);
			keyframeAnimationPart.keyframes = new List<Verse.Keyframe>(framePaths.Count);
			for (int i = 0; i < framePaths.Count; i++)
			{
				GraphicStateDef graphicStateDef = hotReload ? (DefDatabase<GraphicStateDef>.GetNamed(text + "_" + i.ToString(), false) ?? new GraphicStateDef()) : new GraphicStateDef();
				graphicStateDef.defName = text + "_" + i.ToString();
				graphicStateDef.applySkinColorTint = true;
				graphicStateDef.defaultGraphicData = new GraphicData
				{
					graphicClass = typeof(Graphic_Single),
					drawSize = new Vector2(drawSize, drawSize),
					color = (tint ?? Color.white),
					texPath = framePaths[i]
				};
				keyframeAnimationPart.keyframes.Add(new Verse.Keyframe
				{
					graphicState = graphicStateDef,
					tick = i * ticksPerFrame
				});
				AnimationDefGenerator_Flying.generatedGraphicStateDefs.Add(graphicStateDef);
			}
			AnimationDefGenerator_Flying.generatedAnimationDefs.Add(animationDef);
			return animationDef;
		}

		// Token: 0x04003627 RID: 13863
		private static List<GraphicStateDef> generatedGraphicStateDefs = new List<GraphicStateDef>();

		// Token: 0x04003628 RID: 13864
		private static List<AnimationDef> generatedAnimationDefs = new List<AnimationDef>();
	}
}

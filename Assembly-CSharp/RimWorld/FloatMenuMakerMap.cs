using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001D11 RID: 7441
	public static class FloatMenuMakerMap
	{
		// Token: 0x0600C06D RID: 49261 RVA: 0x00462FAC File Offset: 0x004611AC
		public static void Init()
		{
			FloatMenuMakerMap.providers = new List<FloatMenuOptionProvider>();
			foreach (Type type in typeof(FloatMenuOptionProvider).AllSubclassesNonAbstract())
			{
				FloatMenuMakerMap.providers.Add((FloatMenuOptionProvider)Activator.CreateInstance(type));
			}
		}

		// Token: 0x0600C06E RID: 49262 RVA: 0x00463020 File Offset: 0x00461220
		public static List<FloatMenuOption> GetOptions(List<Pawn> selectedPawns, Vector3 clickPos, out FloatMenuContext context)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			context = null;
			if (!clickPos.InBounds(Find.CurrentMap))
			{
				return list;
			}
			context = new FloatMenuContext(selectedPawns, clickPos, Find.CurrentMap);
			if (!context.allSelectedPawns.Any<Pawn>())
			{
				return list;
			}
			if (!context.ClickedCell.IsValid || !context.ClickedCell.InBounds(Find.CurrentMap))
			{
				return list;
			}
			if (!context.IsMultiselect)
			{
				AcceptanceReport acceptanceReport = FloatMenuMakerMap.ShouldGenerateFloatMenuForPawn(context.FirstSelectedPawn);
				if (!acceptanceReport.Accepted)
				{
					if (!acceptanceReport.Reason.NullOrEmpty())
					{
						Messages.Message(acceptanceReport.Reason, context.FirstSelectedPawn, MessageTypeDefOf.RejectInput, false);
					}
					return list;
				}
			}
			else
			{
				context.allSelectedPawns.RemoveAll((Pawn selectedPawn) => !FloatMenuMakerMap.ShouldGenerateFloatMenuForPawn(selectedPawn));
				if (!context.allSelectedPawns.Any<Pawn>())
				{
					return list;
				}
			}
			if (!context.IsMultiselect)
			{
				FloatMenuMakerMap.makingFor = context.FirstSelectedPawn;
			}
			FloatMenuMakerMap.GetProviderOptions(context, list);
			FloatMenuMakerMap.makingFor = null;
			return list;
		}

		// Token: 0x0600C06F RID: 49263 RVA: 0x00463138 File Offset: 0x00461338
		private static void GetProviderOptions(FloatMenuContext context, List<FloatMenuOption> options)
		{
			foreach (FloatMenuOptionProvider floatMenuOptionProvider in FloatMenuMakerMap.providers)
			{
				try
				{
					FloatMenuMakerMap.currentProvider = floatMenuOptionProvider;
					if (context.ValidSelectedPawns.Any<Pawn>())
					{
						if (floatMenuOptionProvider.Applies(context))
						{
							options.AddRange(floatMenuOptionProvider.GetOptions(context));
							foreach (Thing thing in context.ClickedThings)
							{
								if (floatMenuOptionProvider.TargetThingValid(thing, context))
								{
									Thing thing2 = thing;
									CompSelectProxy compSelectProxy;
									if (thing2.TryGetComp(out compSelectProxy) && compSelectProxy.thingToSelect != null)
									{
										thing2 = compSelectProxy.thingToSelect;
									}
									foreach (FloatMenuOption floatMenuOption in floatMenuOptionProvider.GetOptionsFor(thing2, context))
									{
										if (floatMenuOption == null)
										{
											DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 1);
											defaultInterpolatedStringHandler.AppendLiteral("FloatMenuOptionProvider ");
											defaultInterpolatedStringHandler.AppendFormatted<FloatMenuOptionProvider>(floatMenuOptionProvider);
											defaultInterpolatedStringHandler.AppendLiteral(" returned a null option. This is not allowed.");
											Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), floatMenuOptionProvider.GetType().GetHashCode());
										}
										else
										{
											FloatMenuOption floatMenuOption2 = floatMenuOption;
											if (floatMenuOption2.iconThing == null)
											{
												floatMenuOption2.iconThing = thing2;
											}
											floatMenuOption.targetsDespawned = !thing2.Spawned;
											options.Add(floatMenuOption);
										}
									}
								}
							}
							foreach (Pawn pawn in context.ClickedPawns)
							{
								if (floatMenuOptionProvider.TargetPawnValid(pawn, context))
								{
									foreach (FloatMenuOption floatMenuOption3 in floatMenuOptionProvider.GetOptionsFor(pawn, context))
									{
										FloatMenuOption floatMenuOption2 = floatMenuOption3;
										if (floatMenuOption2.iconThing == null)
										{
											floatMenuOption2.iconThing = pawn;
										}
										floatMenuOption3.targetsDespawned = !pawn.Spawned;
										options.Add(floatMenuOption3);
									}
								}
							}
						}
					}
				}
				catch (Exception value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(27, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("Error in FloatMenuWorker ");
					defaultInterpolatedStringHandler2.AppendFormatted(floatMenuOptionProvider.GetType().Name);
					defaultInterpolatedStringHandler2.AppendLiteral(": ");
					defaultInterpolatedStringHandler2.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
			FloatMenuMakerMap.currentProvider = null;
		}

		// Token: 0x0600C070 RID: 49264 RVA: 0x00463440 File Offset: 0x00461640
		public static AcceptanceReport ShouldGenerateFloatMenuForPawn(Pawn pawn)
		{
			if (pawn.Map != Find.CurrentMap)
			{
				return false;
			}
			if (pawn.Downed)
			{
				return "IsIncapped".Translate(pawn.LabelCap, pawn);
			}
			if (ModsConfig.BiotechActive && pawn.Deathresting)
			{
				return "IsDeathresting".Translate(pawn.Named("PAWN"));
			}
			Lord lord = pawn.GetLord();
			if (lord != null)
			{
				AcceptanceReport result = lord.AllowsFloatMenu(pawn);
				if (!result.Accepted)
				{
					return result;
				}
			}
			return true;
		}

		// Token: 0x0600C071 RID: 49265 RVA: 0x004634D8 File Offset: 0x004616D8
		public static FloatMenuOption GetAutoTakeOption(List<FloatMenuOption> options)
		{
			bool flag = true;
			FloatMenuOption floatMenuOption = null;
			foreach (FloatMenuOption floatMenuOption2 in options)
			{
				if (floatMenuOption2.Disabled || !floatMenuOption2.autoTakeable)
				{
					flag = false;
					break;
				}
				if (floatMenuOption == null || floatMenuOption2.autoTakeablePriority > floatMenuOption.autoTakeablePriority)
				{
					floatMenuOption = floatMenuOption2;
				}
			}
			if (!flag || floatMenuOption == null)
			{
				return null;
			}
			return floatMenuOption;
		}

		// Token: 0x04006ED3 RID: 28371
		private static List<FloatMenuOptionProvider> providers;

		// Token: 0x04006ED4 RID: 28372
		public static FloatMenuOptionProvider currentProvider;

		// Token: 0x04006ED5 RID: 28373
		public static Pawn makingFor;
	}
}

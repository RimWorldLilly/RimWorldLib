using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001216 RID: 4630
	public abstract class ComplexThreatWorker
	{
		// Token: 0x060072EA RID: 29418 RVA: 0x00282C80 File Offset: 0x00280E80
		public void Resolve(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings, StringBuilder debug = null)
		{
			try
			{
				bool flag = false;
				if (parms.triggerSignal.NullOrEmpty())
				{
					string triggerSignal;
					if (this.TryGetThingTriggerSignal(parms, out triggerSignal))
					{
						parms.triggerSignal = triggerSignal;
						if (debug != null)
						{
							debug.AppendLine("--> Threat trigger: Existing thing");
						}
					}
					else if (this.def.fallbackToRoomEnteredTrigger)
					{
						parms.triggerSignal = ComplexUtility.SpawnRoomEnteredTrigger(parms.room, parms.map);
						flag = true;
						if (debug != null)
						{
							debug.AppendLine("--> Threat trigger: Room entry");
						}
					}
					else
					{
						if (!this.def.allowPassive)
						{
							Log.Warning("Unable to generate a trigger for threat " + this.def.defName);
							return;
						}
						parms.triggerSignal = this.def.defName + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
						parms.passive = true;
						if (debug != null)
						{
							debug.AppendLine("--> Threat trigger: None. Passive threat.");
						}
					}
				}
				float points = parms.points;
				if (!parms.passive && Rand.Chance(this.def.delayChance))
				{
					int num = this.def.delayTickOptions.RandomElement<int>();
					parms.points = points;
					parms.delayTicks = new int?(num);
					float num2 = this.def.threatFactorOverDelayTicksCurve.Evaluate((float)num);
					parms.points *= num2;
					if (debug != null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
						defaultInterpolatedStringHandler.AppendLiteral("--> Threat delay ticks: ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(num);
						debug.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(32, 1);
						defaultInterpolatedStringHandler2.AppendLiteral("--> Threat delay points factor: ");
						defaultInterpolatedStringHandler2.AppendFormatted<float>(num2);
						debug.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(37, 1);
						defaultInterpolatedStringHandler3.AppendLiteral("--> Threat points post delay factor: ");
						defaultInterpolatedStringHandler3.AppendFormatted<float>(parms.points);
						debug.AppendLine(defaultInterpolatedStringHandler3.ToStringAndClear());
					}
				}
				if (!parms.passive && !flag && Rand.Chance(this.def.spawnInOtherRoomChance))
				{
					ComplexResolveParams parms2 = parms;
					foreach (LayoutRoom room in parms.allRooms.InRandomOrder(null))
					{
						parms2.room = room;
						if (this.def.Worker.CanResolve(parms2))
						{
							parms.room = room;
							break;
						}
					}
				}
				float num3 = 0f;
				this.ResolveInt(parms, ref num3, outSpawnedThings);
				if (parms.passive)
				{
					num3 *= this.def.postSpawnPassiveThreatFactor;
					if (debug != null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(38, 1);
						defaultInterpolatedStringHandler4.AppendLiteral("--> Threat post spawn passive factor: ");
						defaultInterpolatedStringHandler4.AppendFormatted<float>(this.def.postSpawnPassiveThreatFactor);
						debug.AppendLine(defaultInterpolatedStringHandler4.ToStringAndClear());
					}
				}
				if (debug != null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(23, 1);
					defaultInterpolatedStringHandler5.AppendLiteral("--> Total points used: ");
					defaultInterpolatedStringHandler5.AppendFormatted<float>(num3);
					debug.AppendLine(defaultInterpolatedStringHandler5.ToStringAndClear());
				}
				threatPointsUsed += num3;
			}
			catch (Exception ex)
			{
				string str = "Exception resolving ";
				string name = base.GetType().Name;
				string str2 = ": ";
				Exception ex2 = ex;
				Log.Error(str + name + str2 + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x060072EB RID: 29419 RVA: 0x00283000 File Offset: 0x00281200
		private bool TryGetThingTriggerSignal(ComplexResolveParams threatParams, out string triggerSignal)
		{
			if (threatParams.room == null || threatParams.spawnedThings.NullOrEmpty<Thing>())
			{
				triggerSignal = null;
				return false;
			}
			LayoutRoom room = threatParams.room;
			for (int i = 0; i < threatParams.spawnedThings.Count; i++)
			{
				Thing thing = threatParams.spawnedThings[i];
				if (room.rects.Any((CellRect r) => r.Contains(thing.Position)))
				{
					CompHackable compHackable = thing.TryGetComp<CompHackable>();
					if (compHackable != null && !compHackable.IsHacked)
					{
						if (Rand.Bool)
						{
							if (compHackable.hackingStartedSignal == null)
							{
								compHackable.hackingStartedSignal = "ThreatTriggerSignal" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
							}
							triggerSignal = compHackable.hackingStartedSignal;
						}
						else
						{
							if (compHackable.hackingCompletedSignal == null)
							{
								compHackable.hackingCompletedSignal = "ThreatTriggerSignal" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
							}
							triggerSignal = compHackable.hackingCompletedSignal;
						}
						return true;
					}
					Building_Casket building_Casket = thing as Building_Casket;
					if (building_Casket != null && building_Casket.CanOpen)
					{
						if (building_Casket.openedSignal.NullOrEmpty())
						{
							building_Casket.openedSignal = "ThreatTriggerSignal" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
						}
						triggerSignal = building_Casket.openedSignal;
						return true;
					}
				}
			}
			triggerSignal = null;
			return false;
		}

		// Token: 0x060072EC RID: 29420 RVA: 0x00283164 File Offset: 0x00281364
		public bool CanResolve(ComplexResolveParams parms)
		{
			bool result;
			try
			{
				result = this.CanResolveInt(parms);
			}
			catch (Exception ex)
			{
				string str = "Exception test running ";
				string name = base.GetType().Name;
				string str2 = ": ";
				Exception ex2 = ex;
				Log.Error(str + name + str2 + ((ex2 != null) ? ex2.ToString() : null));
				result = false;
			}
			return result;
		}

		// Token: 0x060072ED RID: 29421
		protected abstract void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> spawnedThings);

		// Token: 0x060072EE RID: 29422 RVA: 0x002831C0 File Offset: 0x002813C0
		protected virtual bool CanResolveInt(ComplexResolveParams parms)
		{
			if ((float)this.def.minPoints <= parms.points)
			{
				LayoutRoom room = parms.room;
				if (((room != null) ? room.requiredDef : null) == null)
				{
					return this.def.allowPassive || this.def.fallbackToRoomEnteredTrigger || !parms.triggerSignal.NullOrEmpty();
				}
			}
			return false;
		}

		// Token: 0x040048B6 RID: 18614
		public ComplexThreatDef def;

		// Token: 0x040048B7 RID: 18615
		private const string ThreatTriggerSignal = "ThreatTriggerSignal";
	}
}

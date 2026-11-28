using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001163 RID: 4451
	public class IncidentWorker_MakeGameCondition : IncidentWorker
	{
		// Token: 0x06006FD7 RID: 28631 RVA: 0x00271E69 File Offset: 0x00270069
		public virtual GameConditionDef GetGameConditionDef(IncidentParms parms)
		{
			return this.def.gameCondition;
		}

		// Token: 0x06006FD8 RID: 28632 RVA: 0x00271E78 File Offset: 0x00270078
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = parms.target as Map;
			if (map != null)
			{
				using (List<GameCondition>.Enumerator enumerator = map.gameConditionManager.ActiveConditions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.preventIncidents)
						{
							return false;
						}
					}
				}
			}
			GameConditionManager gameConditionManager = parms.target.GameConditionManager;
			if (gameConditionManager == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Couldn't find condition manager for incident target ");
				defaultInterpolatedStringHandler.AppendFormatted<IIncidentTarget>(parms.target);
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 70849667);
				return false;
			}
			GameConditionDef gameConditionDef = this.GetGameConditionDef(parms);
			if (gameConditionDef == null)
			{
				return false;
			}
			if (gameConditionManager.ConditionIsActive(gameConditionDef))
			{
				return false;
			}
			List<GameCondition> activeConditions = gameConditionManager.ActiveConditions;
			for (int i = 0; i < activeConditions.Count; i++)
			{
				if (!gameConditionDef.CanCoexistWith(activeConditions[i].def))
				{
					return false;
				}
			}
			if (ModsConfig.OdysseyActive && gameConditionDef.requireFish)
			{
				Map map2 = parms.target as Map;
				if (map2 == null || map2.waterBodyTracker == null || !map2.waterBodyTracker.AnyBodyContainsFish)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006FD9 RID: 28633 RVA: 0x00271FBC File Offset: 0x002701BC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			GameConditionManager gameConditionManager = parms.target.GameConditionManager;
			GameConditionDef gameConditionDef = this.GetGameConditionDef(parms);
			int duration = Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f);
			GameCondition gameCondition = GameConditionMaker.MakeCondition(gameConditionDef, duration);
			gameConditionManager.RegisterCondition(gameCondition);
			this.TryFireLetter(parms, gameCondition);
			return true;
		}

		// Token: 0x06006FDA RID: 28634 RVA: 0x00272010 File Offset: 0x00270210
		private void TryFireLetter(IncidentParms parms, GameCondition condition)
		{
			if (!this.def.letterLabel.NullOrEmpty() && !condition.def.letterText.NullOrEmpty())
			{
				Map map = parms.target as Map;
				if (map == null || !condition.HiddenByOtherCondition(map))
				{
					bool flag = false;
					foreach (Map map2 in Find.Maps)
					{
						if (condition.CanApplyOnMap(map2))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return;
					}
					parms.letterHyperlinkThingDefs = condition.def.letterHyperlinks;
					base.SendStandardLetter(this.def.letterLabel, condition.LetterText, this.def.letterDef, parms, LookTargets.Invalid, Array.Empty<NamedArgument>());
					return;
				}
			}
		}
	}
}

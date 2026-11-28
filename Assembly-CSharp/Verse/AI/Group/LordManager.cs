using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000941 RID: 2369
	public sealed class LordManager : IExposable, IDisposable
	{
		// Token: 0x060048CA RID: 18634 RVA: 0x001C8488 File Offset: 0x001C6688
		public LordManager(Map map)
		{
			this.map = map;
			map.events.BuildingSpawned += this.Notify_BuildingSpawned;
			map.events.BuildingDespawned += this.Notify_BuildingDespawned;
		}

		// Token: 0x060048CB RID: 18635 RVA: 0x001C84E8 File Offset: 0x001C66E8
		public void ExposeData()
		{
			Scribe_Collections.Look<Lord>(ref this.lords, "lords", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<StencilDrawerForCells>(ref this.stencilDrawers, "stencilDrawers", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].lordManager = this;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.stencilDrawers == null)
				{
					this.stencilDrawers = new List<StencilDrawerForCells>();
				}
				for (int j = 0; j < this.lords.Count; j++)
				{
					Find.SignalManager.RegisterReceiver(this.lords[j]);
				}
			}
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x001C8598 File Offset: 0x001C6798
		public void LordManagerTick()
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				try
				{
					this.lords[i].LordTick();
				}
				catch (Exception value)
				{
					Lord lord = this.lords[i];
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Exception while ticking lord with job ");
					defaultInterpolatedStringHandler.AppendFormatted((((lord != null) ? lord.LordJob : null) == null) ? "NULL" : lord.LordJob.ToString());
					defaultInterpolatedStringHandler.AppendLiteral(": \r\n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			for (int j = this.lords.Count - 1; j >= 0; j--)
			{
				LordToil curLordToil = this.lords[j].CurLordToil;
				if (curLordToil == null || curLordToil.ShouldFail)
				{
					this.RemoveLord(this.lords[j]);
				}
			}
			for (int k = this.stencilDrawers.Count - 1; k >= 0; k--)
			{
				if (!this.lords.Contains(this.stencilDrawers[k].sourceLord) && this.stencilDrawers[k].ticksLeftWithoutLord <= 0)
				{
					this.stencilDrawers.RemoveAt(k);
				}
			}
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x001C8700 File Offset: 0x001C6900
		public void LordManagerUpdate()
		{
			if (DebugViewSettings.drawLords)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].DebugDraw();
				}
			}
			foreach (StencilDrawerForCells stencilDrawerForCells in this.stencilDrawers)
			{
				stencilDrawerForCells.Draw();
				if (!this.lords.Contains(stencilDrawerForCells.sourceLord))
				{
					stencilDrawerForCells.ticksLeftWithoutLord--;
				}
			}
		}

		// Token: 0x060048CE RID: 18638 RVA: 0x001C87A4 File Offset: 0x001C69A4
		public void LordManagerOnGUI()
		{
			if (DebugViewSettings.drawLords)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].DebugOnGUI();
				}
			}
			if (DebugViewSettings.drawDuties)
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Text.Font = GameFont.Tiny;
				foreach (Pawn pawn in this.map.mapPawns.AllPawns)
				{
					if (pawn.Spawned)
					{
						string text = "";
						if (!pawn.Dead && pawn.mindState.duty != null)
						{
							text = pawn.mindState.duty.ToString();
						}
						if (pawn.InMentalState)
						{
							string str = text;
							string str2 = "\nMentalState=";
							MentalState mentalState = pawn.MentalState;
							text = str + str2 + ((mentalState != null) ? mentalState.ToString() : null);
						}
						Vector2 vector = pawn.DrawPos.MapToUIPosition();
						Widgets.Label(new Rect(vector.x - 100f, vector.y - 100f, 200f, 200f), text);
					}
				}
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		// Token: 0x060048CF RID: 18639 RVA: 0x001C88E4 File Offset: 0x001C6AE4
		public void AddLord(Lord newLord)
		{
			this.lords.Add(newLord);
			newLord.lordManager = this;
			Find.SignalManager.RegisterReceiver(newLord);
			this.map.events.Notify_LordAdded(newLord);
		}

		// Token: 0x060048D0 RID: 18640 RVA: 0x001C8915 File Offset: 0x001C6B15
		public void RemoveLord(Lord oldLord)
		{
			this.map.events.Notify_LordRemoved(oldLord);
			this.lords.Remove(oldLord);
			Find.SignalManager.DeregisterReceiver(oldLord);
			oldLord.Cleanup();
		}

		// Token: 0x060048D1 RID: 18641 RVA: 0x001C8948 File Offset: 0x001C6B48
		public bool TryGetLordByJob<T>(Faction faction, out T lord) where T : LordJob
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				if (this.lords[i].faction == faction)
				{
					T t = this.lords[i].LordJob as T;
					if (t != null)
					{
						lord = t;
						return true;
					}
				}
			}
			lord = default(T);
			return false;
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x001C89B4 File Offset: 0x001C6BB4
		public Lord LordOf(Pawn p)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				Lord lord = this.lords[i];
				for (int j = 0; j < lord.ownedPawns.Count; j++)
				{
					if (lord.ownedPawns[j] == p)
					{
						return lord;
					}
				}
			}
			return null;
		}

		// Token: 0x060048D3 RID: 18643 RVA: 0x001C8A0C File Offset: 0x001C6C0C
		public Lord LordOf(Building b)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				Lord lord = this.lords[i];
				for (int j = 0; j < lord.ownedBuildings.Count; j++)
				{
					if (lord.ownedBuildings[j] == b)
					{
						return lord;
					}
				}
			}
			return null;
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x001C8A64 File Offset: 0x001C6C64
		public Lord LordOf(Corpse c)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				Lord lord = this.lords[i];
				for (int j = 0; j < lord.ownedCorpses.Count; j++)
				{
					if (lord.ownedCorpses[j] == c)
					{
						return lord;
					}
				}
			}
			return null;
		}

		// Token: 0x060048D5 RID: 18645 RVA: 0x001C8ABC File Offset: 0x001C6CBC
		private void Notify_BuildingSpawned(Building b)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				this.lords[i].Notify_BuildingSpawnedOnMap(b);
			}
			BreachingGridDebug.Notify_BuildingStateChanged(b);
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x001C8AF8 File Offset: 0x001C6CF8
		private void Notify_BuildingDespawned(Building b)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				this.lords[i].Notify_BuildingDespawnedOnMap(b);
			}
			BreachingGridDebug.Notify_BuildingStateChanged(b);
		}

		// Token: 0x060048D7 RID: 18647 RVA: 0x001C8B34 File Offset: 0x001C6D34
		public void Notify_MapRemoved()
		{
			for (int i = this.lords.Count - 1; i >= 0; i--)
			{
				this.lords[i].Notify_MapRemoved();
			}
			for (int j = this.lords.Count - 1; j >= 0; j--)
			{
				this.RemoveLord(this.lords[j]);
			}
		}

		// Token: 0x060048D8 RID: 18648 RVA: 0x001C8B94 File Offset: 0x001C6D94
		public void LogLords()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= Lords =======");
			stringBuilder.AppendLine("Count: " + this.lords.Count.ToString());
			for (int i = 0; i < this.lords.Count; i++)
			{
				Lord lord = this.lords[i];
				stringBuilder.AppendLine();
				stringBuilder.Append("#" + (i + 1).ToString() + ": ");
				if (lord.LordJob == null)
				{
					stringBuilder.AppendLine("no-job");
				}
				else
				{
					stringBuilder.AppendLine(lord.LordJob.GetType().Name);
				}
				stringBuilder.Append("Current toil: ");
				if (lord.CurLordToil == null)
				{
					stringBuilder.AppendLine("null");
				}
				else
				{
					stringBuilder.AppendLine(lord.CurLordToil.GetType().Name);
				}
				stringBuilder.AppendLine("Members (count: " + lord.ownedPawns.Count.ToString() + "):");
				for (int j = 0; j < lord.ownedPawns.Count; j++)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					string[] array = new string[5];
					array[0] = "  ";
					array[1] = lord.ownedPawns[j].LabelShort;
					array[2] = " (";
					int num = 3;
					Faction faction = lord.ownedPawns[j].Faction;
					array[num] = ((faction != null) ? faction.ToString() : null);
					array[4] = ")";
					stringBuilder2.AppendLine(string.Concat(array));
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x060048D9 RID: 18649 RVA: 0x001C8D3C File Offset: 0x001C6F3C
		public void Dispose()
		{
			foreach (Lord lord in this.lords)
			{
				lord.Dispose();
			}
		}

		// Token: 0x04002EE3 RID: 12003
		public Map map;

		// Token: 0x04002EE4 RID: 12004
		public List<Lord> lords = new List<Lord>();

		// Token: 0x04002EE5 RID: 12005
		public List<StencilDrawerForCells> stencilDrawers = new List<StencilDrawerForCells>();
	}
}

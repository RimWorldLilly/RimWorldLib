using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D79 RID: 7545
	public static class ITab_Pawn_Log_Utility
	{
		// Token: 0x0600C396 RID: 50070 RVA: 0x00478B2C File Offset: 0x00476D2C
		public static List<ITab_Pawn_Log_Utility.LogLineDisplayable> GenerateLogLinesFor(Pawn pawn, bool showAll, bool showCombat, bool showSocial, int maxLines)
		{
			ITab_Pawn_Log_Utility.tmpLogLines.Clear();
			LogEntry[] array = showSocial ? (from e in Find.PlayLog.AllEntries
			where e.Concerns(pawn)
			select e).ToArray<LogEntry>() : new LogEntry[0];
			int num = 0;
			int num2 = 0;
			Battle battle = null;
			if (!showCombat)
			{
				goto IL_219;
			}
			bool flag = true;
			using (List<Battle>.Enumerator enumerator = Find.BattleLog.Battles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Battle battle2 = enumerator.Current;
					if (battle2.Concerns(pawn))
					{
						foreach (LogEntry logEntry in battle2.Entries)
						{
							if (logEntry.Concerns(pawn))
							{
								if (showAll || logEntry.ShowInCompactView())
								{
									while (num < array.Length && array[num].Age < logEntry.Age)
									{
										if (battle != null && battle != battle2)
										{
											ITab_Pawn_Log_Utility.tmpLogLines.Add(new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding));
											battle = null;
										}
										ITab_Pawn_Log_Utility.tmpLogLines.Add(new ITab_Pawn_Log_Utility.LogLineDisplayableLog(array[num++], pawn));
										num2++;
										if (num2 >= maxLines)
										{
											return ITab_Pawn_Log_Utility.tmpLogLines;
										}
										flag = false;
									}
									if (battle != battle2)
									{
										if (!flag)
										{
											ITab_Pawn_Log_Utility.tmpLogLines.Add(new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding));
										}
										ITab_Pawn_Log_Utility.tmpLogLines.Add(new ITab_Pawn_Log_Utility.LogLineDisplayableHeader(battle2.GetName()));
										battle = battle2;
										flag = false;
									}
									ITab_Pawn_Log_Utility.tmpLogLines.Add(new ITab_Pawn_Log_Utility.LogLineDisplayableLog(logEntry, pawn));
									num2++;
									if (num2 >= maxLines)
									{
										return ITab_Pawn_Log_Utility.tmpLogLines;
									}
								}
							}
						}
					}
				}
				goto IL_219;
			}
			IL_1D3:
			if (battle != null)
			{
				ITab_Pawn_Log_Utility.tmpLogLines.Add(new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding));
				battle = null;
			}
			ITab_Pawn_Log_Utility.tmpLogLines.Add(new ITab_Pawn_Log_Utility.LogLineDisplayableLog(array[num++], pawn));
			num2++;
			if (num2 >= maxLines)
			{
				return ITab_Pawn_Log_Utility.tmpLogLines;
			}
			IL_219:
			if (num >= array.Length)
			{
				return ITab_Pawn_Log_Utility.tmpLogLines;
			}
			goto IL_1D3;
		}

		// Token: 0x04006FCD RID: 28621
		[TweakValue("Interface", 0f, 1f)]
		private static float AlternateAlpha = 0.03f;

		// Token: 0x04006FCE RID: 28622
		[TweakValue("Interface", 0f, 1f)]
		private static float HighlightAlpha = 0.2f;

		// Token: 0x04006FCF RID: 28623
		[TweakValue("Interface", 0f, 10f)]
		private static float HighlightDuration = 4f;

		// Token: 0x04006FD0 RID: 28624
		[TweakValue("Interface", 0f, 30f)]
		private static float BattleBottomPadding = 20f;

		// Token: 0x04006FD1 RID: 28625
		private static List<ITab_Pawn_Log_Utility.LogLineDisplayable> tmpLogLines = new List<ITab_Pawn_Log_Utility.LogLineDisplayable>();

		// Token: 0x02003AD1 RID: 15057
		public class LogDrawData
		{
			// Token: 0x06015312 RID: 86802 RVA: 0x006BC603 File Offset: 0x006BA803
			public void StartNewDraw()
			{
				this.alternatingBackground = false;
			}

			// Token: 0x0400E9A7 RID: 59815
			public bool alternatingBackground;

			// Token: 0x0400E9A8 RID: 59816
			public LogEntry highlightEntry;

			// Token: 0x0400E9A9 RID: 59817
			public float highlightIntensity;
		}

		// Token: 0x02003AD2 RID: 15058
		public abstract class LogLineDisplayable
		{
			// Token: 0x06015314 RID: 86804 RVA: 0x006BC614 File Offset: 0x006BA814
			public float GetHeight(float width)
			{
				if (this.cachedHeight == -1f)
				{
					this.cachedHeight = this.GetHeight_Worker(width);
				}
				return this.cachedHeight;
			}

			// Token: 0x06015315 RID: 86805
			public abstract float GetHeight_Worker(float width);

			// Token: 0x06015316 RID: 86806
			public abstract void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data);

			// Token: 0x06015317 RID: 86807
			public abstract void AppendTo(StringBuilder sb);

			// Token: 0x06015318 RID: 86808 RVA: 0x006BC636 File Offset: 0x006BA836
			public virtual bool Matches(LogEntry log)
			{
				return false;
			}

			// Token: 0x0400E9AA RID: 59818
			private float cachedHeight = -1f;
		}

		// Token: 0x02003AD3 RID: 15059
		public class LogLineDisplayableHeader : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x0601531A RID: 86810 RVA: 0x006BC64C File Offset: 0x006BA84C
			public LogLineDisplayableHeader(TaggedString text)
			{
				this.text = text;
			}

			// Token: 0x0601531B RID: 86811 RVA: 0x006BC65C File Offset: 0x006BA85C
			public override float GetHeight_Worker(float width)
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Medium;
				float result = Text.CalcHeight(this.text, width);
				Text.Font = font;
				return result;
			}

			// Token: 0x0601531C RID: 86812 RVA: 0x006BC68C File Offset: 0x006BA88C
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(0f, position, width, base.GetHeight(width)), this.text);
				Text.Font = GameFont.Small;
			}

			// Token: 0x0601531D RID: 86813 RVA: 0x006BC6B8 File Offset: 0x006BA8B8
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine("--    " + this.text);
			}

			// Token: 0x0400E9AB RID: 59819
			private TaggedString text;
		}

		// Token: 0x02003AD4 RID: 15060
		public class LogLineDisplayableLog : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x0601531E RID: 86814 RVA: 0x006BC6D6 File Offset: 0x006BA8D6
			public LogLineDisplayableLog(LogEntry log, Pawn pawn)
			{
				this.log = log;
				this.pawn = pawn;
			}

			// Token: 0x0601531F RID: 86815 RVA: 0x006BC6EC File Offset: 0x006BA8EC
			public override float GetHeight_Worker(float width)
			{
				float width2 = width - 29f;
				return Mathf.Max(26f, this.log.GetTextHeight(this.pawn, width2) + 4f);
			}

			// Token: 0x06015320 RID: 86816 RVA: 0x006BC724 File Offset: 0x006BA924
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
				float height = base.GetHeight(width);
				float width2 = width - 29f;
				Rect rect = new Rect(0f, position, width, height);
				if (this.log == data.highlightEntry)
				{
					Widgets.DrawRectFast(rect, new Color(1f, 1f, 1f, ITab_Pawn_Log_Utility.HighlightAlpha * data.highlightIntensity), null);
					data.highlightIntensity = Mathf.Max(0f, data.highlightIntensity - Time.deltaTime / ITab_Pawn_Log_Utility.HighlightDuration);
				}
				else if (data.alternatingBackground)
				{
					Widgets.DrawRectFast(rect, new Color(1f, 1f, 1f, ITab_Pawn_Log_Utility.AlternateAlpha), null);
				}
				data.alternatingBackground = !data.alternatingBackground;
				TaggedString label = this.log.ToGameStringFromPOV(this.pawn, false);
				Widgets.Label(new Rect(29f, position, width2, height), label);
				Texture2D texture2D = this.log.IconFromPOV(this.pawn);
				if (texture2D != null)
				{
					GUI.color = (this.log.IconColorFromPOV(this.pawn) ?? Color.white);
					GUI.DrawTexture(new Rect(0f, position + (height - 26f) / 2f, 26f, 26f), texture2D);
					GUI.color = Color.white;
				}
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, () => this.log.GetTipString(), 613261 + this.log.LogID * 2063);
					Widgets.DrawHighlight(rect);
				}
				if (Widgets.ButtonInvisible(rect, this.log.CanBeClickedFromPOV(this.pawn)))
				{
					this.log.ClickedFromPOV(this.pawn);
				}
				if (DebugViewSettings.logCombatLogMouseover && Mouse.IsOver(rect))
				{
					this.log.ToGameStringFromPOV(this.pawn, true);
				}
			}

			// Token: 0x06015321 RID: 86817 RVA: 0x006BC90E File Offset: 0x006BAB0E
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine(this.log.ToGameStringFromPOV(this.pawn, false));
			}

			// Token: 0x06015322 RID: 86818 RVA: 0x006BC929 File Offset: 0x006BAB29
			public override bool Matches(LogEntry log)
			{
				return log == this.log;
			}

			// Token: 0x0400E9AC RID: 59820
			private LogEntry log;

			// Token: 0x0400E9AD RID: 59821
			private Pawn pawn;
		}

		// Token: 0x02003AD5 RID: 15061
		public class LogLineDisplayableGap : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x06015324 RID: 86820 RVA: 0x006BC941 File Offset: 0x006BAB41
			public LogLineDisplayableGap(float height)
			{
				this.height = height;
			}

			// Token: 0x06015325 RID: 86821 RVA: 0x006BC950 File Offset: 0x006BAB50
			public override float GetHeight_Worker(float width)
			{
				return this.height;
			}

			// Token: 0x06015326 RID: 86822 RVA: 0x006BC958 File Offset: 0x006BAB58
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
			}

			// Token: 0x06015327 RID: 86823 RVA: 0x006BC95A File Offset: 0x006BAB5A
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine();
			}

			// Token: 0x0400E9AE RID: 59822
			private float height;
		}
	}
}

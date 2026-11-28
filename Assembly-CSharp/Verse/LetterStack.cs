using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000617 RID: 1559
	public sealed class LetterStack : IExposable
	{
		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x060030B9 RID: 12473 RVA: 0x00138281 File Offset: 0x00136481
		public List<Letter> LettersListForReading
		{
			get
			{
				return this.letters;
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x060030BA RID: 12474 RVA: 0x00138289 File Offset: 0x00136489
		public float LastTopY
		{
			get
			{
				return this.lastTopYInt;
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x060030BB RID: 12475 RVA: 0x00138291 File Offset: 0x00136491
		public BundleLetter BundleLetter
		{
			get
			{
				if (this.bundleLetterCache == null)
				{
					this.bundleLetterCache = (BundleLetter)LetterMaker.MakeLetter(LetterDefOf.BundleLetter);
				}
				return this.bundleLetterCache;
			}
		}

		// Token: 0x060030BC RID: 12476 RVA: 0x001382B8 File Offset: 0x001364B8
		public void ReceiveLetter(TaggedString label, TaggedString text, LetterDef textLetterDef, LookTargets lookTargets, Faction relatedFaction = null, Quest quest = null, List<ThingDef> hyperlinkThingDefs = null, string debugInfo = null, int delayTicks = 0, bool playSound = true)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, lookTargets, relatedFaction, quest, hyperlinkThingDefs);
			this.ReceiveLetter(let, debugInfo, delayTicks, playSound);
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x001382E4 File Offset: 0x001364E4
		public void ReceiveLetter(TaggedString label, TaggedString text, LetterDef textLetterDef, string debugInfo = null, int delayTicks = 0, bool playSound = true)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, null, null);
			this.ReceiveLetter(let, debugInfo, delayTicks, playSound);
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x0013830C File Offset: 0x0013650C
		public void ReceiveLetter(Letter let, string debugInfo = null, int delayTicks = 0, bool playSound = true)
		{
			let.debugInfo = debugInfo;
			if (delayTicks > 0)
			{
				let.arrivalTick = Find.TickManager.TicksGame + delayTicks;
				this.letterQueue.Add(let);
				return;
			}
			if (!let.CanShowInLetterStack)
			{
				return;
			}
			if (playSound)
			{
				let.def.arriveSound.PlayOneShotOnCamera(null);
			}
			if (Prefs.AutomaticPauseMode >= let.def.pauseMode)
			{
				Find.TickManager.Pause();
			}
			else if (let.def.forcedSlowdown)
			{
				Find.TickManager.slower.SignalForceNormalSpeedShort();
			}
			let.arrivalTime = Time.time;
			let.arrivalTick = Find.TickManager.TicksGame;
			this.letters.Add(let);
			Find.Archive.Add(let);
			let.Received();
		}

		// Token: 0x060030BF RID: 12479 RVA: 0x001383D4 File Offset: 0x001365D4
		public void RemoveLetter(Letter let)
		{
			this.letters.Remove(let);
			let.Removed();
		}

		// Token: 0x060030C0 RID: 12480 RVA: 0x001383EC File Offset: 0x001365EC
		public void LettersOnGUI(float baseY)
		{
			float num = baseY;
			float num2 = baseY - Find.Alerts.AlertsHeight;
			float num3 = 42f;
			int num4 = Mathf.FloorToInt(num2 / num3);
			int num5 = Math.Max(this.letters.Count - num4, 0);
			if (num5 > 0)
			{
				num5++;
			}
			for (int i = this.letters.Count - 1; i >= num5; i--)
			{
				num -= 30f;
				this.letters[i].DrawButtonAt(num);
				num -= 12f;
			}
			if (num5 > 0)
			{
				this.tmpBundledLetters.Clear();
				this.tmpBundledLetters.AddRange(this.letters.Take(num5));
				num -= 30f;
				this.BundleLetter.SetLetters(this.tmpBundledLetters);
				this.BundleLetter.DrawButtonAt(num);
				num -= 12f;
				this.tmpBundledLetters.Clear();
			}
			this.lastTopYInt = num;
			if (Event.current.type == EventType.repaint)
			{
				num = baseY;
				for (int j = this.letters.Count - 1; j >= num5; j--)
				{
					num -= 30f;
					this.letters[j].CheckForMouseOverTextAt(num);
					num -= 12f;
				}
				if (num5 > 0)
				{
					num -= 30f;
					this.BundleLetter.CheckForMouseOverTextAt(num);
					num -= 12f;
				}
			}
		}

		// Token: 0x060030C1 RID: 12481 RVA: 0x00138544 File Offset: 0x00136744
		public void OpenAutomaticLetters()
		{
			if (Find.WindowStack.WindowsForcePause)
			{
				return;
			}
			foreach (Letter letter in this.letters)
			{
				if (letter.ShouldAutomaticallyOpenLetter)
				{
					letter.OpenLetter();
					break;
				}
			}
		}

		// Token: 0x060030C2 RID: 12482 RVA: 0x001385B0 File Offset: 0x001367B0
		public void LetterStackTick()
		{
			this.OpenAutomaticLetters();
			for (int i = this.letterQueue.Count - 1; i >= 0; i--)
			{
				if (Find.TickManager.TicksGame >= this.letterQueue[i].arrivalTick)
				{
					this.ReceiveLetter(this.letterQueue[i], null, 0, true);
					this.letterQueue.RemoveAt(i);
				}
			}
		}

		// Token: 0x060030C3 RID: 12483 RVA: 0x0013861C File Offset: 0x0013681C
		public void LetterStackUpdate()
		{
			if (this.mouseoverLetterIndex >= 0 && this.mouseoverLetterIndex < this.letters.Count)
			{
				this.letters[this.mouseoverLetterIndex].lookTargets.TryHighlight(true, true, false);
			}
			this.mouseoverLetterIndex = -1;
			for (int i = this.letters.Count - 1; i >= 0; i--)
			{
				if (!this.letters[i].CanShowInLetterStack)
				{
					this.RemoveLetter(this.letters[i]);
				}
			}
		}

		// Token: 0x060030C4 RID: 12484 RVA: 0x001386A7 File Offset: 0x001368A7
		public void Notify_LetterMouseover(Letter let)
		{
			this.mouseoverLetterIndex = this.letters.IndexOf(let);
		}

		// Token: 0x060030C5 RID: 12485 RVA: 0x001386BC File Offset: 0x001368BC
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.letters.Count; i++)
			{
				if (this.letters[i].relatedFaction == faction)
				{
					this.letters[i].relatedFaction = null;
				}
			}
		}

		// Token: 0x060030C6 RID: 12486 RVA: 0x00138708 File Offset: 0x00136908
		public void ExposeData()
		{
			Scribe_Collections.Look<Letter>(ref this.letters, "letters", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Letter>(ref this.letterQueue, "letterQueue", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.letterQueue == null)
				{
					this.letterQueue = new List<Letter>();
				}
				this.letters.RemoveAll((Letter x) => x == null);
				this.letterQueue.RemoveAll((Letter x) => x == null);
			}
		}

		// Token: 0x04001F62 RID: 8034
		private List<Letter> letters = new List<Letter>();

		// Token: 0x04001F63 RID: 8035
		private List<Letter> letterQueue = new List<Letter>();

		// Token: 0x04001F64 RID: 8036
		private int mouseoverLetterIndex = -1;

		// Token: 0x04001F65 RID: 8037
		private float lastTopYInt;

		// Token: 0x04001F66 RID: 8038
		private BundleLetter bundleLetterCache;

		// Token: 0x04001F67 RID: 8039
		private const float LettersBottomY = 350f;

		// Token: 0x04001F68 RID: 8040
		public const float LetterSpacing = 12f;

		// Token: 0x04001F69 RID: 8041
		private List<Letter> tmpBundledLetters = new List<Letter>();
	}
}

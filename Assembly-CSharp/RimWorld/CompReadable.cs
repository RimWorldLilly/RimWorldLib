using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EAA RID: 3754
	public class CompReadable : ThingComp
	{
		// Token: 0x170010D2 RID: 4306
		// (get) Token: 0x060062D3 RID: 25299 RVA: 0x00239178 File Offset: 0x00237378
		public IEnumerable<ReadingOutcomeDoer> Doers
		{
			get
			{
				return this.doers;
			}
		}

		// Token: 0x170010D3 RID: 4307
		// (get) Token: 0x060062D4 RID: 25300 RVA: 0x00239180 File Offset: 0x00237380
		public CompProperties_Readable Props
		{
			get
			{
				return (CompProperties_Readable)this.props;
			}
		}

		// Token: 0x060062D5 RID: 25301 RVA: 0x0023918D File Offset: 0x0023738D
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.InitializeDoers();
		}

		// Token: 0x060062D6 RID: 25302 RVA: 0x0023919C File Offset: 0x0023739C
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.InitializeDoers();
			if (this.doers == null)
			{
				return;
			}
			for (int i = 0; i < this.doers.Count; i++)
			{
				this.doers[i].PostMake();
			}
		}

		// Token: 0x060062D7 RID: 25303 RVA: 0x002391E8 File Offset: 0x002373E8
		public bool TryGetDoer<T>(out T doer) where T : ReadingOutcomeDoer
		{
			if (this.doers == null)
			{
				doer = default(T);
				return false;
			}
			for (int i = 0; i < this.doers.Count; i++)
			{
				T t = this.doers[i] as T;
				if (t != null)
				{
					doer = t;
					return true;
				}
			}
			doer = default(T);
			return false;
		}

		// Token: 0x060062D8 RID: 25304 RVA: 0x0023924C File Offset: 0x0023744C
		public T GetDoer<T>() where T : ReadingOutcomeDoer
		{
			if (this.doers == null)
			{
				return default(T);
			}
			for (int i = 0; i < this.doers.Count; i++)
			{
				T t = this.doers[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060062D9 RID: 25305 RVA: 0x002392AB File Offset: 0x002374AB
		public IEnumerable<T> GetDoers<T>() where T : ReadingOutcomeDoer
		{
			CompReadable.<GetDoers>d__9<T> <GetDoers>d__ = new CompReadable.<GetDoers>d__9<T>(-2);
			<GetDoers>d__.<>4__this = this;
			return <GetDoers>d__;
		}

		// Token: 0x060062DA RID: 25306 RVA: 0x002392BC File Offset: 0x002374BC
		private void InitializeDoers()
		{
			if (this.Props.doers.Empty<ReadingOutcomeProperties>())
			{
				return;
			}
			this.doers = new List<ReadingOutcomeDoer>();
			for (int i = 0; i < this.Props.doers.Count; i++)
			{
				ReadingOutcomeDoer readingOutcomeDoer = null;
				ReadingOutcomeProperties readingOutcomeProperties = this.Props.doers[i];
				try
				{
					readingOutcomeDoer = (ReadingOutcomeDoer)Activator.CreateInstance(readingOutcomeProperties.DoerClass);
					this.doers.Add(readingOutcomeDoer);
					readingOutcomeDoer.Initialize(this.parent, readingOutcomeProperties);
				}
				catch (Exception value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Could not instantiate or initialize reading outcome doer (");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(readingOutcomeProperties.DoerClass);
					defaultInterpolatedStringHandler.AppendLiteral("): ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					this.doers.Remove(readingOutcomeDoer);
				}
			}
		}

		// Token: 0x060062DB RID: 25307 RVA: 0x002393AC File Offset: 0x002375AC
		public override void PostExposeData()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeDoers();
			}
			if (this.doers == null)
			{
				return;
			}
			for (int i = 0; i < this.doers.Count; i++)
			{
				this.doers[i].PostExposeData();
			}
		}

		// Token: 0x0400368A RID: 13962
		protected List<ReadingOutcomeDoer> doers;
	}
}

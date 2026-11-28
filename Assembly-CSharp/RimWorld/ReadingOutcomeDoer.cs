using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB7 RID: 3767
	public abstract class ReadingOutcomeDoer
	{
		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x06006326 RID: 25382 RVA: 0x0023A6A4 File Offset: 0x002388A4
		// (set) Token: 0x06006327 RID: 25383 RVA: 0x0023A6AC File Offset: 0x002388AC
		public ThingWithComps Parent { get; private set; }

		// Token: 0x170010E5 RID: 4325
		// (get) Token: 0x06006328 RID: 25384 RVA: 0x0023A6B5 File Offset: 0x002388B5
		// (set) Token: 0x06006329 RID: 25385 RVA: 0x0023A6BD File Offset: 0x002388BD
		public CompReadable Readable { get; private set; }

		// Token: 0x170010E6 RID: 4326
		// (get) Token: 0x0600632A RID: 25386 RVA: 0x0023A6C6 File Offset: 0x002388C6
		public ReadingOutcomeProperties Props
		{
			get
			{
				return this.props;
			}
		}

		// Token: 0x0600632B RID: 25387 RVA: 0x0023A6CE File Offset: 0x002388CE
		public virtual void Initialize(ThingWithComps parent, ReadingOutcomeProperties props)
		{
			this.Parent = parent;
			this.Readable = parent.GetComp<CompReadable>();
			this.props = props;
		}

		// Token: 0x0600632C RID: 25388 RVA: 0x0023A6EA File Offset: 0x002388EA
		public virtual void PostMake()
		{
		}

		// Token: 0x0600632D RID: 25389 RVA: 0x0023A6EC File Offset: 0x002388EC
		public virtual void OnReadingTick(Pawn reader, float factor)
		{
		}

		// Token: 0x0600632E RID: 25390 RVA: 0x0023A6EE File Offset: 0x002388EE
		public virtual void PostExposeData()
		{
		}

		// Token: 0x040036A2 RID: 13986
		protected ReadingOutcomeProperties props;
	}
}

using System;

namespace Verse.Sound
{
	// Token: 0x020007CA RID: 1994
	public class SoundParameterMapping
	{
		// Token: 0x06003FC1 RID: 16321 RVA: 0x00197D3C File Offset: 0x00195F3C
		public SoundParameterMapping()
		{
			this.curve = new SimpleCurve();
			this.curve.Add(new CurvePoint(0f, 0f), true);
			this.curve.Add(new CurvePoint(1f, 1f), true);
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x00197D90 File Offset: 0x00195F90
		public void Apply(Sample samp)
		{
			if (this.inParam == null || this.outParam == null)
			{
				return;
			}
			float x = this.inParam.ValueFor(samp);
			float value = this.curve.Evaluate(x);
			this.outParam.SetOn(samp, value);
		}

		// Token: 0x04002A49 RID: 10825
		[Description("The independent parameter that the game will change to drive this relationship.\n\nOn the graph, this is the X axis.")]
		public SoundParamSource inParam;

		// Token: 0x04002A4A RID: 10826
		[Description("The dependent parameter that will respond to changes to the in-parameter.\n\nThis must match something the game can change about this sound.\n\nOn the graph, this is the y-axis.")]
		public SoundParamTarget outParam;

		// Token: 0x04002A4B RID: 10827
		[Description("Determines when sound parameters should be applies to samples.\n\nConstant means the parameters are updated every frame and can change continuously.\n\nOncePerSample means that the parameters are applied exactly once to each sample that plays.")]
		public SoundParamUpdateMode paramUpdateMode;

		// Token: 0x04002A4C RID: 10828
		[EditorHidden]
		public SimpleCurve curve;
	}
}

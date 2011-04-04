using System;
using System.Collections.Generic;
using System.Windows;
using MavenThought.Commons.Testing;
using Price.Displacement.Domain.Calculations.DraftRatio;
using Price.Displacement.Infrastructure.Comfort;
using Rhino.Mocks;

namespace GeorgeChen.MavenThought_VSExtension
{
    /// <summary>
    /// Base specification for BilenearGradient
    /// </summary>
    public abstract class MaventThought
        : AutoMockSpecificationWithNoContract<BilinearGradient>
    {
        /// <summary>
        /// comfort values used
        /// </summary>
        private readonly IList<IColorQuadrant> _colorQuadrants = new List<IColorQuadrant>
                                                                     {
                                                                        MockIColorQuadrant(), 
                                                                        MockIColorQuadrant(), 
                                                                        MockIColorQuadrant(), 
                                                                        MockIColorQuadrant()
                                                                     };

        protected Action CancelCallback { get; set; }

        protected Action<IEnumerable<DataPoint>> DoneCallback { get; set; }

        /// <summary>
        /// the resolution to use as input
        /// </summary>
        protected int Resolution { get; set; }

        /// <summary>
        /// Inject dependencies in sut
        /// </summary>
        /// <returns></returns>
        protected override BilinearGradient CreateSut()
        {
            return new BilinearGradient(this.Resolution,
                                        Dep<IBitmapGenerator>(),
                                        Dep<IColorQuadrantGenerator>(),
                                        Dep<IRangeComfortColorCalculator>());
        }

        /// <summary>
        /// Given these dependencies
        /// </summary>
        protected override void GivenThat()
        {
            base.GivenThat();

            Dep<IColorQuadrantGenerator>()
                .Stub(c => c.GetEnumerator())
                .Return(null)
                .WhenCalled(mi => mi.ReturnValue = _colorQuadrants.GetEnumerator());

            DoneCallback = MockIt(DoneCallback);

            CancelCallback = MockIt(CancelCallback);

            this.Resolution = 77;
        }

        private static IColorQuadrant MockIColorQuadrant()
        {
            var quadrant = Mock<IColorQuadrant>();
            quadrant.Stub(q => q.Comfort).Return(MockComfortQuadrant());
            return quadrant;
        }

        private static IComfortQuadrant MockComfortQuadrant()
        {
            var quadrant = Mock<IComfortQuadrant>();
            quadrant.Stub(q => q.TopLeft).Return(MockComfortPoint());
            quadrant.Stub(q => q.TopRight).Return(MockComfortPoint());
            quadrant.Stub(q => q.BottomRight).Return(MockComfortPoint());
            quadrant.Stub(q => q.BottomLeft).Return(MockComfortPoint());
            quadrant.Stub(q => q.Bounds).Return(new Rect());
            return quadrant;
        }

        private static IComfortPoint MockComfortPoint()
        {
            var cv = Mock<IComfortPoint>();
            cv.Stub(c => c.ComfortValue).Return(MockComfortValue());
            return cv;
        }

        private static IComfortValue MockComfortValue()
        {
            var value = Mock<IComfortValue>();
            value.Stub(v => v.DraftRatio).Return(new Random(DateTime.Now.Millisecond).NextDouble());
            return value;
        }
    }
}
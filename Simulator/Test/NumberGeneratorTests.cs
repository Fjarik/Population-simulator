﻿using System;
using System.Collections.Generic;
using System.Text;
using Core.Generators;
using NUnit.Framework;
using SharedLibrary.Interfaces.Generator;

namespace Test
{
	public class NumberGeneratorTests
	{
		private INumberGenerator _numberGenerator;

		[SetUp]
		public void Setup()
		{
			_numberGenerator = new NumberGenerator();
		}

		[Test]
		[TestCase(0.5D, 0.1D)]
		public void GetDoubleAround(double around, double precision = 0.1)
		{
			var res = this._numberGenerator.GetDoubleAround(around, precision);

			Assert.IsTrue(res >= around - precision && res <= around + precision);
		}

		[Test]
		[TestCase(0D, 10D)]
		public void GetRandomDouble(double min, double max)
		{
			var res = this._numberGenerator.GetRandomDouble(min, max);

			Assert.IsTrue(res >= min && res <= max);
		}

		[Test]
		[TestCase(1D, 1D, 0D, 1D, 0.1D)]
		[TestCase(0.5D, 0.5D, 0D, 0.5D, 0.1D)]
		[TestCase(1D, 1D, 0D, 0.5D, 0.5D)]
		[TestCase(1D, 1D, 0D, 0.5D, 1D)]
		[TestCase(1D, 1D, 1D, 0D, 0.1D)]
		public void CalculateModifier(double mother,
									  double father,
									  double degradation,
									  double expected,
									  double mutation = 0.1)
		{
			var res = this._numberGenerator.CalculateModifier(mother, father, degradation, mutation);

			Assert.IsTrue((expected - mutation) <= res);
		}

		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(10)]
		[TestCase(1000)]
		public void GetRandomInt(int max)
		{
			Assert.GreaterOrEqual(max, 0);

			var res = this._numberGenerator.GetRandomInt(max);

			Assert.LessOrEqual(res, max);
		}

		[Test]
		[TestCase(0, 1)]
		[TestCase(0, 10)]
		[TestCase(20, 100)]
		public void GetRandomInt(int min, int max)
		{
			Assert.GreaterOrEqual(max, 0);
			Assert.IsTrue(min < max);

			var res = this._numberGenerator.GetRandomInt(min, max);

			Assert.IsTrue(min <= res && res <= max);
		}

		public void GetMalesCount(int total)
		{
			throw new NotImplementedException();
		}

		public void GetChildrenCount(double fatherPotency, double motherPotency)
		{
			throw new NotImplementedException();
		}
	}
}
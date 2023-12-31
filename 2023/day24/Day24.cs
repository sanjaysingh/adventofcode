using Microsoft.Z3;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
namespace AdventOfCode;
public static class Day24
{
    private static readonly string inputFileFullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "day24/input.txt");

    public static void SolvePart1()
    {
        var hailstones = ReadInput();
        const decimal minTestArea = 200000000000000;
        const decimal maxTestArea = 400000000000000;
        decimal pathCrossingCount = 0;
        for (var i = 0; i < hailstones.Count; i++)
        {
            for (var j = i + 1; j < hailstones.Count; j++)
            {
                var intersection = hailstones[i].Intersect(hailstones[j]);
                if (intersection != null && 
                    hailstones[i].IsInFuture(intersection) && 
                    hailstones[j].IsInFuture(intersection)  && 
                    intersection.IsInRange(minTestArea, maxTestArea)) 
                    pathCrossingCount++;
            }
        }

        Console.WriteLine(pathCrossingCount);
    }

    public static void SolvePart2()
    {
        // Reference https://pastebin.com/fkpZWn8X

        var hailstones = ReadInput();
        var ctx = new Context();
        var solver = ctx.MkSolver();

        // Coordinates of the stone
        var rx = ctx.MkIntConst("rx");
        var ry = ctx.MkIntConst("ry");
        var rz = ctx.MkIntConst("rz");

        // Velocity of the stone
        var rvx = ctx.MkIntConst("rvx");
        var rvy = ctx.MkIntConst("rvy");
        var rvz = ctx.MkIntConst("rvz");

        // feed some input to the solver. As low as 3 hailstone should be enoigh. However it was taking long time, Bigger input seems to perform well
        for (var i = 0; i < 15; i++)
        {
            var t = ctx.MkIntConst($"t{i}"); // time for the stone to reach the hail
            var hail = hailstones[i];

            var hx = ctx.MkInt(Convert.ToInt64(hail.StartPos.X));
            var hy = ctx.MkInt(Convert.ToInt64(hail.StartPos.Y));
            var hz = ctx.MkInt(Convert.ToInt64(hail.StartPos.Z));

            var hvx = ctx.MkInt(Convert.ToInt64(hail.Velocity.X));
            var hvy = ctx.MkInt(Convert.ToInt64(hail.Velocity.Y));
            var hvz = ctx.MkInt(Convert.ToInt64(hail.Velocity.Z));

            var xLeft = ctx.MkAdd(rx, ctx.MkMul(t, rvx)); // rx + t * rvx
            var yLeft = ctx.MkAdd(ry, ctx.MkMul(t, rvy)); // ry + t * rvy
            var zLeft = ctx.MkAdd(rz, ctx.MkMul(t, rvz)); // rz + t * rvz

            var xRight = ctx.MkAdd(hx, ctx.MkMul(t, hvx)); // hx + t * hvx
            var yRight = ctx.MkAdd(hy, ctx.MkMul(t, hvy)); // hy + t * hvy
            var zRight = ctx.MkAdd(hz, ctx.MkMul(t, hvz)); // hz + t * hvz

            solver.Add(t >= 0); // time should always be positive - we don't want solutions for negative time
            solver.Add(ctx.MkEq(xLeft, xRight)); 
            solver.Add(ctx.MkEq(yLeft, yRight)); 
            solver.Add(ctx.MkEq(zLeft, zRight)); 
        }

        solver.Check();
        var model = solver.Model;

        var rxVal = model.Eval(rx);
        var ryVal = model.Eval(ry);
        var rzVal = model.Eval(rz);

        var result =  Convert.ToInt64(rxVal.ToString()) + Convert.ToInt64(ryVal.ToString()) + Convert.ToInt64(rzVal.ToString());

        Console.WriteLine(result);
    }

    private static bool IsInRange(this Vector point, decimal min, decimal max) => point.X >= min && point.X <= max && point.Y >= min && point.Y <= max;
    private static List<HailStonePath> ReadInput() =>
        File.ReadAllLines(inputFileFullPath).Select(line =>
        {
            var parts = line.Split('@', ',').Select(x => decimal.Parse(x.Trim())).ToArray();
            return new HailStonePath(new Vector(parts[0], parts[1], parts[2]), new Vector(parts[3], parts[4], parts[5]));
        }).ToList();
    private record Vector(Decimal X, Decimal Y, Decimal Z);
    private record HailStonePath(Vector StartPos, Vector Velocity)
    {
        public Vector NextPos => new Vector(StartPos.X + Velocity.X, StartPos.Y + Velocity.Y, StartPos.Z + Velocity.Z);
        public decimal Gradient => Velocity.Y / Velocity.X;

        public decimal C => StartPos.Y - (StartPos.X * Gradient);

        public bool IsInFuture(Vector intersection) => (intersection.X - this.StartPos.X) / this.Velocity.X > 0;
        public Vector? Intersect(HailStonePath other)
        {
            // y = m1x + c1 //line 1
            // y = m2x + c2 //line 2
            // m1x + c1 = m2x + c2
            // (m1-m2)x = (c2-c1)
            // x = (c2-c1) / (m1-m2) // this is x for point of intersection. Y can be caclulated as mx+c using this x.

            if (this.Gradient == other.Gradient) return null;
            var x = (other.C - this.C) / (this.Gradient - other.Gradient);
            var y = (this.Gradient * x) + this.C;
            return new Vector(x, y, 0);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// U.I. Gupta, D.T. Lee, J.-T Leung. An optimal solution for the channel-assignment problem; IEEE Transactions on Computers C-28(1979), 807-810.
//
// Citation of the authors: "Given a set of intervals (pairs of real numbers), we look at the problem of finding a minimal partition of this set such that no element of the partition contains two overlapping intervals. We exhibit a O (n log n) algorithm which is optimal. The problem has applications in LSI layout design and job scheduling".
//
// c.f. http://www-sop.inria.fr/members/Sid.Touati/sw/MSSIG/

namespace Interval
{
    public class Interval<T> where T : IComparable
    {
        private T left, right;

        public T Left
        {
            get { return this.left; }
        }
        public T Right
        {
            get { return this.right; }
        }
        private int chan;
        public int Channel
        {
            get { return this.chan; }
            set { this.chan = value; }
        }

        public Interval(T l, T r) {
            left = l;
            right = r;
            chan = 0;
        }
    }

    internal class Endpoint<T> : IComparable<Endpoint<T>> where T : IComparable 
    {
        private T pt;
        private bool isLeft;
        private int intervalId;

        public T Point
        {
            get { return this.pt; }
        }
        public bool IsLeft
        {
            get { return this.isLeft; }
        }
        public int IntervalId
        {
            get { return this.intervalId; }
        }
        public Endpoint(T p, int id, bool isl)
        {
            pt = p;
            intervalId = id;
            isLeft = isl;
        }

        public int CompareTo(Endpoint<T> v)
        {
            T p1 = this.pt;
            T p2 = v.pt;

            int r = pt.CompareTo(p2);
            if (r != 0)
                return r;

            if (isLeft && !v.isLeft) return 1;
            if (isLeft && v.isLeft) return 0;
            if (!isLeft && v.isLeft) return -1;
            return 0;
        }
    }

    public class MaximalStableSet
    {
        public static int partition<T>(ICollection<Interval<T>> intervals) where T : IComparable
        {
            int N = intervals.Count;
            if (N == 0)
                return 0;
            Endpoint<T>[] endpoints = new Endpoint<T>[N * 2];

            int[] next = new int[N];
            int i = 0, j = 0;
            int temp;
            int maxchan = 0, counter = 0, chan = 0;

            foreach (Interval<T> iv in intervals)
            {
                endpoints[2 * i] = new Endpoint<T>(iv.Left, i, true);
                endpoints[2 * i + 1] = new Endpoint<T>(iv.Right, i, false);
                i++;
            }

            Array.Sort(endpoints);

            for (i = 0; i < N; i++)
            {
                next[i] = i + 1;
            }

            for (j = 0; j < 2 * N; j++)
            {
                Endpoint<T> zj = endpoints[j];
                if (zj.IsLeft)
                {
                    counter++;
                    maxchan = Math.Max(counter, maxchan);
                    intervals.ElementAt(zj.IntervalId).Channel = chan;
                    chan = next[chan];
                }
                else
                {
                    counter--;
                    temp = intervals.ElementAt(zj.IntervalId).Channel;
                    next[temp] = chan;
                    chan = temp;
                }
            }

            return maxchan;
        }
    }
}

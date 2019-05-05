using System;

namespace GlLib.Utils
{
    public class RestrictedVector3D
    {
        public double x;
        public double y;
        public int z;

        public RestrictedVector3D()
        {
        }

        public RestrictedVector3D(double _x, double _y, int _z)
        {
            (x, y, z) = (_x, _y, _z);
        }

        public int Ix => (int) Math.Floor(x);
        public int Iy => (int) Math.Floor(y);

        public static RestrictedVector3D operator +(RestrictedVector3D _a, RestrictedVector3D _b)
        {
            if (_a.z != _b.z)
                throw new ArgumentException("Tried to sum vectors with different heights");
            return new RestrictedVector3D(_a.x + _b.x, _a.y + _b.y, _a.z);
        }

        public static RestrictedVector3D operator +(RestrictedVector3D _a, PlanarVector _b)
        {
            return new RestrictedVector3D(_a.x + _b.x, _a.y + _b.y, _a.z);
        }

        public static RestrictedVector3D operator -(RestrictedVector3D _a)
        {
            return new RestrictedVector3D(-_a.x, -_a.y, _a.z);
        }

        public static bool operator ==(RestrictedVector3D _a, RestrictedVector3D _b)
        {
            if (_a is null || _b is null)
                return false;
            return _b.Equals(_a);
        }

        public static bool operator !=(RestrictedVector3D _a, RestrictedVector3D _b)
        {
            return !(_b == _a);
        }

        public static RestrictedVector3D operator -(RestrictedVector3D _a, PlanarVector _b)
        {
            return _a + -_b;
        }

        public static RestrictedVector3D operator -(RestrictedVector3D _a, RestrictedVector3D _b)
        {
            if (_a.z != _b.z)
                throw new ArgumentException("Tried to subtract vectors with different heights");
            return _a + -_b;
        }

        public static RestrictedVector3D operator *(RestrictedVector3D _a, double _k)
        {
            return new RestrictedVector3D(_a.x * _k, _a.y * _k, _a.z);
        }

        public static RestrictedVector3D operator *(double _k, RestrictedVector3D _a)
        {
            return new RestrictedVector3D(_a.x * _k, _a.y * _k, _a.z);
        }

        public static RestrictedVector3D FromAngleAndHeight(double _angle, int _height)
        {
            return new RestrictedVector3D(Math.Cos(_angle), Math.Sin(_angle), _height);
        }

        public RestrictedVector3D Rotate(double _angle)
        {
            var cos = Math.Cos(_angle);
            var sin = Math.Sin(_angle);
            return new RestrictedVector3D(x * cos - y * sin, x * sin + y * cos, z);
        }

        public override string ToString()
        {
            return $"({x},{y},{z})";
        }

        public static RestrictedVector3D FromString(string _s)
        {
            if (_s == "")
                return new RestrictedVector3D();
            var coords = _s.Substring(1, _s.Length - 2).Split(",");
            return new RestrictedVector3D(double.Parse(coords[0]), double.Parse(coords[1]), int.Parse(coords[2]));
        }

        public override bool Equals(object _obj)
        {
            var item = _obj as RestrictedVector3D;
            if (item is null)
                return false;
            return Equals(item);
        }

        protected bool Equals(RestrictedVector3D _item)
        {
            return Math.Abs(_item.x - x) < 1e-3
                   && Math.Abs(_item.y - y) < 1e-3
                   && _item.z == z;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z;
                return hashCode;
            }
        }

        public PlanarVector ToPlanar()
        {
            return new PlanarVector(x, y);
        }
    }

    public class PlanarVector
    {
        public double x;
        public double y;

        public PlanarVector()
        {
        }

        public PlanarVector(double _x, double _y)
        {
            (x, y) = (_x, _y);
        }

        public double Length => Math.Sqrt(x * x + y * y);

        public static PlanarVector operator *(PlanarVector _a, double _k)
        {
            return new PlanarVector(_a.x * _k, _a.y * _k);
        }

        public static PlanarVector operator /(PlanarVector _a, double _k)
        {
            return new PlanarVector(_a.x / _k, _a.y / _k);
        }

        public static PlanarVector operator +(PlanarVector _a, PlanarVector _b)
        {
            return new PlanarVector(_a.x + _b.x, _a.y + _b.y);
        }
        // TODO operator '-'

        public static bool operator ==(PlanarVector _a, PlanarVector _b)
        {
            if (_a is null || _b is null)
                return false;
            return _b.Equals(_a);
        }

        public static bool operator !=(PlanarVector _a, PlanarVector _b)
        {
            return !(_b == _a);
        }

        public static PlanarVector operator -(PlanarVector _a)
        {
            return new PlanarVector(-_a.x, -_a.y);
        }

        public override string ToString()
        {
            return $"({x},{y})";
        }

        public static PlanarVector FromString(string _s)
        {
            if (_s == "")
                return new PlanarVector();
            var coords = _s.Substring(1, _s.Length - 2).Split(",");
            return new PlanarVector(double.Parse(coords[0]), double.Parse(coords[1]));
        }

        public AxisAlignedBb Expand(double _width, double _height)
        {
            return new AxisAlignedBb(x, y, x + _width, y + _height);
        }

        public override bool Equals(object _obj)
        {
            var item = _obj as PlanarVector;
            if (item is null)
                return false;
            return Equals(item);
        }

        protected bool Equals(PlanarVector _item)
        {
            return Math.Abs(_item.x - x) < 1e-3
                   && Math.Abs(_item.y - y) < 1e-3;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x.GetHashCode() * 397) ^ y.GetHashCode();
            }
        }

        public AxisAlignedBb ExpandBothTo(double _width, double _height)
        {
            return new AxisAlignedBb(x - _width / 2, y / _height, x + _width / 2, y + _height / 2);
        }
    }

    public class AxisAlignedBb
    {
        public double endX;
        public double endY;
        public double startX;
        public double startY;

        public AxisAlignedBb(double _startX, double _startY, double _endX, double _endY)
        {
            (startX, startY, endX, endY) =
                (_startX, _startY, _endX, _endY);
            CheckCoordinates();
        }

        public AxisAlignedBb(PlanarVector _start, PlanarVector _end)
        {
            (startX, startY, endX, endY) = (_start.x, _start.y, _end.x, _end.y);
            CheckCoordinates();
        }

        public int StartXi => (int) startX;
        public int StartYi => (int) startY;
        public int EndXi => (int) endX;
        public int EndYi => (int) endY;

        public double Width => endX - startX;
        public double Height => endY - startY;

        public bool IsVectorInside(PlanarVector _vector)
        {
            return _vector.x <= endX && _vector.x >= startX && _vector.y <= endY && _vector.y >= startY;
        }

        public bool IntersectsWith(AxisAlignedBb _box)
        {
            var cx1 = (startX + endX) / 2;
            var cy1 = (startY + endY) / 2;
            var cx2 = (_box.startX + _box.endX) / 2;
            var cy2 = (_box.startY + _box.endY) / 2;

            var halfWidth = endX - cx1 + _box.endX - cx2;
            var halfHeight = endY - cy1 + _box.endY - cy2;

            return Math.Abs(cx1 - cx2) <= halfWidth && Math.Abs(cy1 - cy2) <= halfHeight;
        }

        public static AxisAlignedBb operator +(AxisAlignedBb _a, PlanarVector _v)
        {
            return new AxisAlignedBb(_a.startX + _v.x, _a.startY + _v.y, _a.endX + _v.x, _a.endY + _v.y);
        }


        public override bool Equals(object _obj)
        {
            var item = _obj as AxisAlignedBb;
            if (item is null)
                return false;
            return Equals(item);
        }

        protected bool Equals(AxisAlignedBb _item)
        {
            return Math.Abs(_item.startX - startX) < 1e-3
                   && Math.Abs(_item.startY - startY) < 1e-3
                   &&Math.Abs(_item.endX - endX) < 1e-3
                   &&Math.Abs(_item.endY - endY) < 1e-3;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = endX.GetHashCode();
                hashCode = (hashCode * 397) ^ endY.GetHashCode();
                hashCode = (hashCode * 397) ^ startX.GetHashCode();
                hashCode = (hashCode * 397) ^ startY.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            //TODO
            return $"Bounding {startX},{startY},{EndXi},{EndYi}";
        }

        private void CheckCoordinates()
        {
            if (startX > endX) (startX, endX) = (endX, startY);

            if (startY > endY) (startY, endY) = (endY, startY);
        }
    }
}

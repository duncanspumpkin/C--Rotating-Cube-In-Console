using System;
using System.Linq;
using System.Collections.Generic;

class Cubes
{
    static void Main()
    {

        List<Point3D> corners = new List<Point3D>{
           new Point3D(-1,-1,-1),
           new Point3D(1,-1,-1),           
           new Point3D(1,-1,1),
           new Point3D(-1,-1,1),
           new Point3D(-1,1,1),
           new Point3D(-1,1,-1),
           new Point3D(1,1,-1),
           new Point3D(1,1,1)};
        //Found this on stackoverflow for making all the lines its quite clever
        //as it selects only once each line
        var lines = from a in corners
                    from b in corners
                    where (a - b).Length == 2 && a.x + a.y + a.z > b.x + b.y + b.z
                    select new { a, b };

        //This angle looks nice
        float angX = 208.0f, angY = 154.0f, angZ = 0.0f;
        Console.WriteLine("Hi");
        while (true)
        {
            //Console.WriteLine("angZ={0}",angZ);
            foreach (var line in lines)
            {
                //Console.WriteLine( "line: {0},{1},{2}",line.a.x,line.a.y,line.a.z );
                //Console.WriteLine( "line: {0},{1},{2}",line.b.x,line.b.y,line.b.z );
                for (int i = 0; i < 25; i++)
                {
                    //Find a point between A and B by following formula p=a+z(b-a) where z
                    //is a value between 0 and 1.
                    //The 24 and the 25 in this loop are magic resolution numbers
                    var point = line.a + (i * 1.0f / 24) * (line.b - line.a);
                    //Console.WriteLine("x:{0},y:{1},z:{2}",point.x,point.y,point.z);
                    //Rotates the point relative to all the angles.
                    Point3D r = point.rotateX(angX).rotateY(angY).rotateZ(angZ);
                    //Projects the point into 2d space. Magic numbers are fun
                    Point3D q = r.project(30, 30, 256, 4);
                    //Hmm there is a reason behind these numbers i have a feeling it was
                    //the only ones that centered things right.
                    Console.SetCursorPosition(((int)q.x + 150) / 10, ((int)q.y + 150) / 10);
                    Console.Write('#');
                }
            }
            //Rotate our cube a bit more
            angX += 1.0f;
            angY += 1.0f;
            angZ += 1.0f;
            //Console.ReadKey();
            System.Threading.Thread.Sleep(100);
            Console.Clear();
        }
    }
}


class Point3D
{
    public float x;
    public float y;
    public float z;

    public Point3D(float X, float Y, float Z)
    {
        x = X;
        y = Y;
        z = Z;
    }

    public Point3D rotateX(float angle)
    {
        float rad = angle * (float)Math.PI / 180;
        float cosa = (float)Math.Cos(rad);
        float sina = (float)Math.Sin(rad);
        float tempY = y;
        y = y * cosa - z * sina;
        z = tempY * sina + z * cosa;
        return this;
    }

    public Point3D rotateY(float angle)
    {
        float rad = angle * (float)Math.PI / 180;
        float cosa = (float)Math.Cos(rad);
        float sina = (float)Math.Sin(rad);
        float tempX = x;
        x = z * sina + x * cosa;
        z = z * cosa - tempX * sina;
        return this;
    }

    public Point3D rotateZ(float angle)
    {
        float rad = angle * (float)Math.PI / 180;
        float cosa = (float)Math.Cos(rad);
        float sina = (float)Math.Sin(rad);
        float tempX = x;
        x = x * cosa - y * sina;
        y = y * cosa + tempX * sina;
        return this;
    }

    public Point3D project(float width, float height, float fov, float viewDist)
    {
        float factor = fov / (viewDist + z);
        Point3D p = new Point3D(x, y, z);
        return new Point3D(p.x * factor + width / 2, -p.y * factor + height / 2, 1);
    }

    public float Length { get { return (float)Math.Sqrt(x * x + y * y + z * z); } }
    public static Point3D operator *(float scale, Point3D x)
    {
        Point3D p = new Point3D(x.x, x.y, x.z);
        p.x *= scale;
        p.y *= scale;
        p.z *= scale;
        return p;
    }

    public static Point3D operator -(Point3D left, Point3D right)
    {
        Point3D p = new Point3D(left.x, left.y, left.z);
        p.x -= right.x;
        p.y -= right.y;
        p.z -= right.z;
        return p;
    }

    public static Point3D operator +(Point3D left, Point3D right)
    {
        Point3D p = new Point3D(left.x, left.y, left.z);
        p.x += right.x;
        p.y += right.y;
        p.z += right.z;
        return p;
    }
}

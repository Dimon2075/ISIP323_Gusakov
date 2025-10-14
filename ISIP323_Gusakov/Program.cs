public abstract class Person
{
    public int ID { get; protected set; }
    public string Name { get; protected set; }
    public int Age { get; protected set; }
    public string Email { get; protected set; }
    protected Person(int id, string name, int age, string email)
    {

    }
    public abstract string GetDetails();
}
public class Student : Person
{
    public List<Course> Courses { get; private set; }
    public Student(int id, string name, int age, string email) : base(id, name, age, email)
    {
        Courses = new List<Course>();
    }
    public void EnrollInCourse(Course course) { }
    public void UnenrollFromCourse(Course course) { }
    public override string GetDetails() { }
}
public class Teacher : Person
{
    public List<Course> Courses { get; private set; }
    public Teacher(int id, string name, int age, string email) : base (id, name, age, email)
    {
        Courses = new List<Course>();
    }
    public override string GetDetails() { }
}
public class Course
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public Teacher Instructor { get; private set; }
    public List<Student> EnrolledStudents { get; private set; }
    public Course(int id, string name, Teacher instructor)
    {

    }
    public void AddStudent(Student student) { }
    public void RemoveStudent(Student student) { }
    public string GetDetails() { }
}
public class Universitity
{
    private List<Student> students;
    private List<Teacher> teachers;
    private List<Course> courses;
    public Universitity()
    {

    }
    public void AddStudent(Student student) { }
    public void RemoveStudent(int studentID) { }
    public Student GetStudent(int id) { }
    public void AddTeacher(Teacher teacher) { }
    public void RemoveTeacher(int id) { }
    public Teacher GetTeacher(int id) { }
    public void AddCourse(Course course) { }
    public void RemoveCourse(int id) { }
    public Course GetCourse(int id) { }
    public IEnumerable<Student> GetAllStudents() { }
    public IEnumerable<Teacher> GetAllTeachers() { }
    public IEnumerable<Course> GetAllCourses() { }
}
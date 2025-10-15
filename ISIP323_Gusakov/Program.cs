using System;
using System.Collections.Generic;
using System.Linq;
public abstract class Person //Содержит все без реализации
{
    public int ID { get; protected set; } //Досутпен внутри класса и к производным классам
    public string Name { get; protected set; }
    public int Age { get; protected set; }
    public string Email { get; protected set; }
    protected Person(int id, string name, int age, string email)
    {
        ID = id;
        Name = name;
        Age = age; 
        Email = email;
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
    public void EnrollInCourse(Course course) 
    {
        if(!Courses.Contains(course))
        {
            Courses.Add(course);
        }
    }
    public void UnenrollFromCourse(Course course) 
    {
        if (!Courses.Contains(course))
        {
            Courses.Remove(course);
        }
    }
    public override string GetDetails() 
    {
        return $"ID {ID}, Студент: {Name}, Возраст: {Age}, Email: {Email}";
    }
}
public class Teacher : Person
{
    public List<Course> Courses { get; private set; }
    public Teacher(int id, string name, int age, string email) : base (id, name, age, email)
    {
        Courses = new List<Course>();
    }
    public override string GetDetails() 
    {
        return $"ID {ID}, Преподователь: {Name}, Возраст {Age}, Email {Email}";
    }
}
public class Course
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public Teacher Instructor { get; set; }
    public List<Student> EnrolledStudents { get; private set; }
    public Course(int id, string name, Teacher instructor)
    {
        ID = id;
        Name = name;
        Instructor = instructor;
        EnrolledStudents = new List<Student>();
    }
    public void AddStudent(Student student) 
    { 
        if(!EnrolledStudents.Contains(student))
        {
            EnrolledStudents.Add(student);
        }
    }
    public void RemoveStudent(Student student) 
    {
        if (!EnrolledStudents.Contains(student))
        {
            EnrolledStudents.Remove(student);
        }
    }
    public string GetDetails()
    {
        return $"(ID {ID}), Курс {Name}, Преподователь: {Instructor?.Name ?? "Нет"}";
    }
    
}
public class Universitity
{
    private List<Student> students;
    private List<Teacher> teachers;
    private List<Course> courses;
    public Universitity()
    {
        students = new List<Student>();
        teachers = new List<Teacher>();
        courses = new List<Course>();
    }
    public void AddStudent(Student student) 
    {
        if (!students.Any(s => s.ID == student.ID))
                students.Add(student);
    }
    public void RemoveStudent(int id) 
    {
        var student = GetStudent(id);
        if(student != null)
        {
            foreach (var course in courses)
                course.RemoveStudent(student);
            students.Remove(student);
        }
    }
    public Student GetStudent(int id) => students.FirstOrDefault(s => s.ID == id);//Возвращает первый элемент из посл.
    public void AddTeacher(Teacher teacher) 
    {
        if (!teachers.Any(t => t.ID == teacher.ID))
            teachers.Add(teacher);
    }
    public void RemoveTeacher(int id) 
    {
        var teacher = GetTeacher(id);
        if (teacher != null)
        {
            var teacherCourses = courses.Where(c => c.Instructor != null && c.Instructor.ID == teacher.ID).ToList();
            foreach (var course in teacherCourses)
                course.Instructor = null;
            teachers.Remove(teacher);
        }
    }
    public Teacher GetTeacher(int id) => teachers.FirstOrDefault(t => t.ID == id);
    public void AddCourse(Course course) 
    {
        if (!courses.Any(c => c.ID == course.ID))
            courses.Add(course);
    }
    public void RemoveCourse(int id) 
    {
        var course = GetCourse(id);
        if (course != null)
        {
            foreach (var student in course.EnrolledStudents)
                student.UnenrollFromCourse(course);
            courses.Remove(course);
        }
    }
    public Course GetCourse(int id) => courses.FirstOrDefault(c => c.ID == id);
    public IEnumerable<Student> GetAllStudents() => students; //Перебор элементов в коллекцию
    public IEnumerable<Teacher> GetAllTeachers() => teachers;
    public IEnumerable<Course> GetAllCourses() => courses;
}
public class Pr5
{
    static Universitity universitity = new Universitity();
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Меню");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Добавить преподователя");
            Console.WriteLine("3. Создать курс");
            Console.WriteLine("4. Записать студента на курс");
            Console.WriteLine("5. Посмотреть всех студентов");
            Console.WriteLine("6. Просмотреть все курсы");
            Console.WriteLine("7. Просмотреть всех преподователей");
            Console.WriteLine("8. Показать детали курса");
            Console.WriteLine("9. Депнуть студента");
            Console.WriteLine("10. Удалить курс");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine();
            switch(choice)
            {
                case "1":
                    AddStudent();
                    break;
                case "2":
                    AddTeacher();
                    break;
                case "3":
                    CreateCourse();
                    break;
                case "4":
                    EnrolledStudentInCourse();
                    break;
                case "5":
                    ShowAllStudents();
                    break;
                case "6":
                    ShowAllCourses();
                    break;
                case "7":
                    ShowAllTeachers();
                    break;
                case "8":
                    ShowCourseDetails();
                    break;
                case "9":
                    RemoveStudent();
                    break;
                case "10":
                    RemoveCourse();
                    break;
                case "0":
                    return;
                    default: Console.WriteLine("Неккоректный выбор.");break;
            }
        }
    }
    static int GetIntInput(string promt)
    {
        int value;
        Console.WriteLine(promt);
        while(!int.TryParse(Console.ReadLine(), out value))
        {
            Console.Write("Ошибка! НОВОЕ ЧИСЛО ВВЕДИ ");
        }
        return value;
    }
    static void AddStudent() //Добавление студента
    {
        int id = GetIntInput("Введите ID студента: ");
        Console.Write("Введите имя: ");
        string name = Console.ReadLine();
        int age = GetIntInput("Введите возраст: ");
        Console.Write("Введите email: ");
        string email = Console.ReadLine();
        var student = new Student(id, name, age, email);
        universitity.AddStudent(student);
        Console.WriteLine("Студент добавлен.");
    }
    static void AddTeacher() //Добавление препода
    {
        int id = GetIntInput("Введите ID преподователя: ");
        Console.Write("Введите имя: ");
        string name = Console.ReadLine();
        int age = GetIntInput("Введите возраст: ");
        Console.Write("Введите email: ");
        string email = Console.ReadLine();
        var teacher = new Teacher(id, name, age, email);
        universitity.AddTeacher(teacher);
        Console.WriteLine("Преподователь добавлен.");
    }
    static void CreateCourse() //Создать курс
    {
        int id = GetIntInput("Введите ID курса: ");
        Console.Write("Введите название курса: ");
        string name = Console.ReadLine();
        Console.WriteLine("Доступные преподователи:");
        foreach (var t in universitity.GetAllTeachers())
            Console.WriteLine($"ID: {t.ID}, Имя: {t.Name}");
        int teacherID = GetIntInput("Введите ID препода для курса: ");
        var instuctor = universitity.GetTeacher(teacherID);
        if (instuctor == null)
        {
            Console.WriteLine("Препод не найден. Создай нового...");
            return;
        }
        var course = new Course(id, name, instuctor);
        universitity.AddCourse(course);
        Console.WriteLine("Курс СОЗДАН.");
    }
    static void EnrolledStudentInCourse() //Записать студента на курс
    {
        Console.WriteLine("Доступные студенты:");
        foreach (var s in universitity.GetAllStudents())
            Console.WriteLine($"ID: {s.ID}, Имя: {s.Name}");
        int studentID = GetIntInput("Введи ID студента: ");
        var student = universitity.GetStudent(studentID);
        if (student == null)
        {
            Console.WriteLine("Студент был депнут");
            return;
        }
        Console.WriteLine("Доступные курсы:");
        foreach(var c in universitity.GetAllCourses())
            Console.WriteLine($"ID: {c.ID}, Название {c.Name}");
        int courseID = GetIntInput("Введите ID курса: ");
        var course = universitity.GetCourse(courseID);
        if (course == null)
        {
            Console.WriteLine("Курс депнули"); 
            return;
        }
        course.AddStudent(student);
        student.EnrollInCourse(course);
        Console.WriteLine($"Студент {student.Name}, записан на курс {course.Name}.");
    }
    static void ShowAllStudents() //Просмотр студентов
    {
        Console.WriteLine("Все студенты:");
        foreach (var s in universitity.GetAllStudents())
            Console.WriteLine(s.GetDetails());
    }
    static void ShowAllCourses() //Просмотр курсов
    {
        Console.WriteLine("Все курсы:");
        foreach (var c in universitity.GetAllCourses())
            Console.WriteLine(c.GetDetails());
    }
    static void ShowAllTeachers() //Просмотр преподов
    {
        Console.WriteLine("Все преподы:");
        foreach (var t in universitity.GetAllTeachers())
            Console.WriteLine(t.GetDetails());
    }
    static void ShowCourseDetails() //Просмотр деталей курса
    {
        int id = GetIntInput("Введит ID курса: ");
        var course = universitity.GetCourse(id);
        if(course == null)
        {
            Console.WriteLine("Курс депнули");
            return;
        }
        Console.WriteLine(course.GetDetails());
        Console.WriteLine("Студенты на курсе:");
        foreach(var s in course.EnrolledStudents)
            Console.WriteLine(s.GetDetails());
    }
    static void RemoveStudent() //Депнуть студента
    {
        int id = GetIntInput("Введи ID студента");
        universitity.RemoveStudent(id);
        Console.WriteLine("Yes. Депнули студента");
    }
    static void RemoveCourse() //Депнуть курс
    {
        int id = GetIntInput("Введи ID курса");
        universitity.RemoveCourse(id);
        Console.WriteLine("Yes. Депнули курс");
    }
}
# Схеми до лабораторної роботи 3.3 - варіант 2

## 1. Архітектура рішення

```mermaid
---
config:
  layout: elk
---
flowchart TD
    U(["Користувач"]) -->|"вводить команди"| APP

    subgraph APP["Lab3.App"]
        P["Program.Main"]
    end

    subgraph PL["Lab3.PL - рівень представлення"]
        M["Menu.MainMenu"]
        EM["EntityMenu: Student, TaxiDriver, Acrobat"]
        V["View-класи та IViewMapper"]
    end

    subgraph BLL["Lab3.BLL - бізнес-логіка"]
        S["EntityService"]
        MD["Моделі, Validator, власні винятки"]
        EMP["IEntityMapper"]
    end

    subgraph DAL["Lab3.DAL - доступ до даних"]
        C["EntityContext"]
        DP["DataProvider: Binary, Custom, Xml, Json"]
        E["Entity-класи"]
    end

    F[("Файл БД: .bin, .txt, .xml, .json")]

    P -->|"викликає MainMenu"| M
    M --> EM
    EM --> V
    EM -->|"дані, тип і ім'я файлу"| S
    S --> MD
    S --> EMP
    S -->|"викликає"| C
    C -->|"обирає провайдера"| DP
    DP --> E
    DP -->|"читає і пише"| F
```

## 2. Діаграми класів

### 2.1. Рівень DAL: сутності, контекст, провайдери

```mermaid
---
config:
  layout: elk
---
classDiagram
    direction TB

    class Entity {
        <<abstract>>
        +GetKey() string
        +GetObjectData(info, context)
    }

    class PersonEntity {
        <<abstract>>
        +string FirstName
        +string LastName
        +int Dances
    }

    class StudentEntity {
        +int Course
        +string StudentId
        +string ArrivalCity
        +string Passport
    }

    class TaxiDriverEntity {
        +string DriverLicense
    }

    class AcrobatEntity {
        +string CertificateId
        +int ExperienceYears
    }

    class EntityContext~T~ {
        +string FilePath
        +bool IsOpen
        +Open(path, type) bool
        +Add(item)
        +Remove(item) bool
        +Update(item)
        +SaveChanges()
        +SaveAs(path, type)
        +Refresh()
        +Close()
        +Delete()
    }

    class DataProvider~T~ {
        <<abstract>>
        +SerializationType Type
        +string FileExtension
        +Save(path, items)
        +Load(path) T[]
        #Write(stream, items)*
        #Read(stream) T[]*
    }

    class BinaryDataProvider~T~
    class CustomDataProvider~T~
    class XmlDataProvider~T~
    class JsonDataProvider~T~

    class DataProviderFactory {
        <<static>>
        +Create(type) DataProvider
        +ExtensionFor(type) string
    }

    class DataAccessException

    Entity <|-- PersonEntity : успадковує
    PersonEntity <|-- StudentEntity : успадковує
    PersonEntity <|-- TaxiDriverEntity : успадковує
    PersonEntity <|-- AcrobatEntity : успадковує

    DataProvider~T~ <|-- BinaryDataProvider~T~ : успадковує
    DataProvider~T~ <|-- CustomDataProvider~T~ : успадковує
    DataProvider~T~ <|-- XmlDataProvider~T~ : успадковує
    DataProvider~T~ <|-- JsonDataProvider~T~ : успадковує

    EntityContext~T~ ..> DataProvider~T~ : читає і пише файл
    EntityContext~T~ ..> DataProviderFactory : отримує провайдера
    EntityContext~T~ ..> Entity : зберігає набір сутностей
    DataProviderFactory ..> DataProvider~T~ : створює
    DataProvider~T~ ..> DataAccessException : повідомляє про помилки
```

### 2.2. Рівень BLL: моделі, сервіси, винятки

```mermaid
---
config:
  layout: elk
---
classDiagram
    direction TB

    class IDancer {
        <<interface>>
        +Dance()
    }

    class Person {
        <<abstract>>
        +string FirstName
        +string LastName
        +int Dances
        +string Key
        +Dance()
    }

    class Student {
        +int Course
        +string StudentId
        +string ArrivalCity
        +string Passport
    }

    class TaxiDriver {
        +string DriverLicense
    }

    class Acrobat {
        +string CertificateId
        +int ExperienceYears
    }

    class IEntityService~TModel~ {
        <<interface>>
        +OpenStorage(path, format) bool
        +SaveAs(path, format)
        +CloseStorage()
        +DeleteStorage()
        +GetAll() TModel[]
        +Add(model)
        +Remove(key)
        +GetByKey(key) TModel
        +FindByLastName(lastName) TModel[]
        +Dance(key)
    }

    class EntityService~TModel~ {
        -EntityContext context
        -IEntityMapper mapper
    }

    class StudentService {
        +GetFirstYearFromOtherCities(city) FirstYearReport
    }

    class IEntityMapper~TModel~ {
        <<interface>>
        +ToModel(entity) TModel
        +ToEntity(model) Entity
    }

    class ServiceFactory {
        <<static>>
        +CreateStudentService() StudentService
        +CreateTaxiDriverService() IEntityService
        +CreateAcrobatService() IEntityService
    }

    class Validator {
        <<static>>
        +IsValid(pattern, value) bool
        +Require(pattern, value, field)
    }

    class BllException
    class EntityNotFoundException
    class DuplicateEntityException
    class InvalidEntityDataException
    class StorageException

    IDancer <|.. Person : реалізує
    Person <|-- Student : успадковує
    Person <|-- TaxiDriver : успадковує
    Person <|-- Acrobat : успадковує

    IEntityService~TModel~ <|.. EntityService~TModel~ : реалізує
    EntityService~TModel~ <|-- StudentService : успадковує
    EntityService~TModel~ ..> IEntityMapper~TModel~ : перетворює DAL і BLL
    EntityService~TModel~ ..> Person : працює з моделями
    ServiceFactory ..> StudentService : створює сервіс і контекст
    Person ..> Validator : перевіряє дані
    EntityService~TModel~ ..> BllException : генерує винятки

    BllException <|-- EntityNotFoundException : успадковує
    BllException <|-- DuplicateEntityException : успадковує
    BllException <|-- InvalidEntityDataException : успадковує
    BllException <|-- StorageException : успадковує
```

### 2.3. Рівень PL і точка входу

```mermaid
---
config:
  layout: elk
---
classDiagram
    direction TB

    class Program {
        -Main()
    }

    class Menu {
        <<static>>
        +MainMenu()
    }

    class EntityMenu~TView~ {
        <<abstract>>
        +Run()
        #ReadView() TView
        #SampleViews() TView[]
        #ExtraItems() MenuItem[]
    }

    class StudentMenu {
        -RunVariantTask()
    }

    class TaxiDriverMenu
    class AcrobatMenu

    class ConsoleIO {
        <<static>>
        +ReadLine(prompt) string
        +ReadValid(label, pattern, hint) string
        +Choose(title, options) int
    }

    class IView {
        <<interface>>
        +Describe() string
    }

    class StudentView
    class TaxiDriverView
    class AcrobatView

    class IViewMapper~TView~ {
        <<interface>>
        +ToView(model) TView
        +ToModel(view) Model
    }

    class IEntityService~TModel~ {
        <<interface>>
    }

    Program ..> Menu : викликає MainMenu
    Menu ..> EntityMenu~TView~ : запускає підменю
    EntityMenu~TView~ <|-- StudentMenu : успадковує
    EntityMenu~TView~ <|-- TaxiDriverMenu : успадковує
    EntityMenu~TView~ <|-- AcrobatMenu : успадковує
    EntityMenu~TView~ ..> ConsoleIO : вводить і виводить
    EntityMenu~TView~ ..> IEntityService~TModel~ : викликає методи сервісу
    EntityMenu~TView~ ..> IViewMapper~TView~ : перетворює дані
    IView <|.. StudentView : реалізує
    IView <|.. TaxiDriverView : реалізує
    IView <|.. AcrobatView : реалізує
```

### 2.4. Частина 1: клас Book та серіалізатори

```mermaid
---
config:
  layout: elk
---
classDiagram
    direction TB

    class Program {
        -Main()
    }

    class Book {
        +int SerialNumber
        +string Title
        +int PublicationYear
        +decimal PricePerCopy
        +int Copies
        +IncreasePrice(percent)
        +TotalCost() decimal
        +GetInfo() string
        +GetObjectData(info, context)
    }

    class ISerializable {
        <<interface>>
        +GetObjectData(info, context)
    }

    class IBookSerializer {
        <<interface>>
        +string Name
        +string Extension
        +Serialize(path, books)
        +Deserialize(path) TCollection
    }

    class BinaryBookSerializer
    class CustomBookSerializer
    class XmlBookSerializer
    class JsonBookSerializer

    Program ..> IBookSerializer : серіалізує масив і List
    Program ..> Book : створює чотири книги
    ISerializable <|.. Book : реалізує
    IBookSerializer <|.. BinaryBookSerializer : реалізує
    IBookSerializer <|.. CustomBookSerializer : реалізує
    IBookSerializer <|.. XmlBookSerializer : реалізує
    IBookSerializer <|.. JsonBookSerializer : реалізує
    IBookSerializer ..> Book : записує і відновлює
    CustomBookSerializer ..> ISerializable : читає дані через GetObjectData
```

## 3. Взаємодія об'єктів: додавання студента

```mermaid
sequenceDiagram
    actor User as Користувач
    participant Menu as EntityMenu (PL)
    participant VM as StudentViewMapper (PL)
    participant Svc as EntityService (BLL)
    participant Map as StudentMapper (BLL)
    participant Ctx as EntityContext (DAL)
    participant Prov as DataProvider (DAL)
    participant File as Файл БД

    User->>Menu: пункт 3 - Додати
    loop для кожного поля
        Menu->>User: запит значення
        User->>Menu: введення
        Note over Menu: перевірка регулярним виразом
    end
    Menu->>VM: ToModel(StudentView)
    VM-->>Menu: Student (модель BLL перевіряє дані)
    Menu->>Svc: Add(student)
    Svc->>Svc: перевірка унікальності ключа
    Svc->>Map: ToEntity(student)
    Map-->>Svc: StudentEntity
    Svc->>Ctx: Add(entity)
    Svc->>Ctx: SaveChanges()
    Ctx->>Prov: Save(path, items)
    Prov->>File: серіалізація обраним способом
    File-->>Prov: записано
    Prov-->>Ctx: готово
    Ctx-->>Svc: готово
    Svc-->>Menu: готово
    Menu-->>User: Додано

    Note over Menu,Svc: Помилки (BllException) піднімаються до меню, і лише воно їх обробляє
```

## 4. Вибір провайдера за типом серіалізації

```mermaid
---
config:
  layout: elk
---
flowchart TD
    A(["Користувач обрав формат і ім'я файлу в меню"]) --> B["PL передає SerializationFormat і шлях в EntityService"]
    B --> C["BLL перетворює формат на SerializationType і додає розширення, якщо його немає"]
    C --> D["EntityContext.Open: DataProviderFactory.Create"]
    D --> E{"Який тип?"}
    E -->|"Binary"| F["BinaryDataProvider"]
    E -->|"Custom"| G["CustomDataProvider"]
    E -->|"Xml"| H["XmlDataProvider"]
    E -->|"Json"| I["JsonDataProvider"]
    F --> J{"Файл існує?"}
    G --> J
    H --> J
    I --> J
    J -->|"так"| K["Load: прочитати сутності з файлу"]
    J -->|"ні"| L["Save: створити порожній файл"]
    K --> M(["Файл відкрито"])
    L --> M
```

## 5. Завдання варіанта: відсоток студентів 1-го курсу з інших міст

```mermaid
---
config:
  layout: elk
---
flowchart TD
    A(["Початок"]) --> B{"Файл БД відкрито?"}
    B -->|"ні"| X["StorageException: файл не відкрито"]
    B -->|"так"| C["Перевірити назву міста університету"]
    C --> D["Перечитати дані з файлу: Refresh"]
    D --> E["Відібрати студентів 1-го курсу"]
    E --> F["Відібрати серед них тих, чиє місто прибуття не збігається з містом університету"]
    F --> G{"Студентів 1-го курсу немає?"}
    G -->|"так"| H["Відсоток = 0"]
    G -->|"ні"| I["Відсоток = кількість з інших міст / кількість 1-го курсу * 100"]
    H --> J(["Повернути звіт: кількості, відсоток, дані студентів"])
    I --> J
```

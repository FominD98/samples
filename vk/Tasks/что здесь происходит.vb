''Задание - объясни "что здесь происходит"?
'
''в этом задании надо рассмотреть код, и добавить комментарии где есть ' для чего этот код или что происходит по ходу выполнения кода
'
''----------------------------------------------------------------
''Берет с сессии вебформ объект и вызывает у него метод и сохраняет изменения, при ошибке логирует в фаил
'Public Sub Example1(ByVal uidPersonWantsOrg As String, ByVal uidPersonHead As String, ByVal bDecision As Boolean, ByVal strText As String)
'    '
'    Dim dbPersonWantsOrg As IEntity
'    '
'    dbPersonWantsOrg = Session.Source.Get("PersonWantsOrg", uidPersonWantsOrg)
'    '
'    Try
'    '
'        dbPersonWantsOrg.CallMethod("MakeDecisionEx", uidPersonHead, bDecision, strText)
'    '
'        dbPersonWantsOrg.Save(Session)
'    
'    Catch ex As Exception
'    '
'        VID_Write2Log("C:\Test.log", ViException.ErrorString(ex))
'    
'    End Try
'    
'End Sub
'
''----------------------------------------------------------------
''Создает коллекцию персон и сохраняет каждого в сессии
'Public Sub Example2()
'    '
'    Dim colPersons As IEntityCollection
'    Dim dbPerson As IEntity
'    Dim f As ISqlFormatter = Session.SqlFormatter
'    '
'    Dim qPerson = Query.From("Person") _
'                  .SelectDisplays()
'
'    colPersons = Session.Source.GetCollection(qPerson)
'    '
'    For Each colElement As IEntity In colPersons
'    '
'        dbPerson = colElement.Create(Session, EntityLoadType.Interactive)
'    Next
'End Sub
'
''----------------------------------------------------------------
''Найти количество персон с именем Paula
'Public Sub Example3()
'    '
'    Dim qPerson = Query.From("Person") _
'                  .Where(Function(c) c.Column("FirstName") = "Paula").SelectCount()
'    '
'    Dim iCount = Session.Source.GetCount(qPerson)
'End Sub
'
''----------------------------------------------------------------
''Обновляем UID_Locality на positions, если значения отличаются 
'Public Sub Example4()
'    '
'    Dim positions = FGetPosition().objects
'    '
'    Dim f As ISqlFormatter = Session.SqlFormatter
'    Dim colPersons As IEntityCollection
'    '
'    Dim table = Connection.Tables("Person")
'    'ищем FK из Person
'    Dim columnUID_Locality As New ResolveImportValueHashed(
'        Connection,
'        ObjectWalker.ColDefs(table, "FK(UID_Locality).CustomProperty04"), False)
'    '
'    Dim query As Query = Query.From("Person").SelectNone()
'    'выбираем поля из Person
'    query = query.Select("CentralAccount")
'    query = query.Select("UID_Locality")
'    '
'    query = query.Where("EntryDate Is Not null")
'    query = query.Where("IsInActive=0")
'    '
'    colPersons = Session.Source.GetCollection(query)
'
''
'    For Each colElement As IEntity In colPersons
'        Dim valUID_Locality As String
'        Dim position As TSPosition
'        'ищем позицию по username и x.office not null
'        If positions.Exists(Function(x) x.user.username = colElement.GetValue("CentralAccount") And Not IsNothing(x.office)) Then
'            position = positions.First(Function(x) x.user.username = colElement.GetValue("CentralAccount") And Not IsNothing(x.office))
'            valUID_Locality = "office" + position.office.id
'        Else
'        '   если не нашли, ищем дальше
'            Continue For
'        End If
'
'        'конвертим в строку
'        Dim resUID_Locality As String
'        resUID_Locality = DbVal.ConvertTo(Of String)(columnUID_Locality.ResolveValue(valUID_Locality))
'
'        '
'        Dim entity As IEntity = Nothing
'        'ленивое создание новой сущности
'        Dim fullEntity As New Lazy(Of IEntity)(Function() colElement.Create(Session))
'
'        'проверяем UID_Locality и columnUID_Locality, если отличаются устанавливаем новое значение
'        If DbVal.Compare(colElement.GetRaw("UID_Locality"), resUID_Locality, ValType.String) <> 0 Then
'            fullEntity.Value.PutValue("UID_Locality", resUID_Locality)
'        End If
'
'        'если создан, то присваиваем значение
'        If fullEntity.IsValueCreated Then
'        '
'            entity = fullEntity.Value
'        End If
'        'если не null и отличается от сохраненной в базе, то сохраняем в сессии 
'        If Not entity Is Nothing Then
'        '
'            If entity.IsDifferent Then
'            '
'                entity.Save(Session)
'            End If
'        End If
'
'
'    Next
'
'End Sub
'
'
'
'#If Not SCRIPTDEBUGGER Then
'	Imports TStem.Collections.Generic
'	Imports TStem.Data
'	Imports TStem.Net
'	Imports TStem.IO
'    'исключить лишние импорты при отладке
'	Imports Newtonsoft.Json
'#End If
'
'Public Function FGetPosition() As PositionList
'    '
'    Dim apimethod As String = "/positions/"
'    Dim host = "https://targetTStem.example.ru/REST"
'    Dim page As String = apimethod
'    Dim uri = host & page
'    Dim positionlist As New TSPositionList
'    Dim position As New List(Of TSPosition)
'    Dim meta As New TSMeta
'    '
'    positionlist.objects = position
'    positionlist.meta = meta
'    '
'    While Not String.IsNullOrEmpty(page)
'        uri = host & page
'        '
'        Dim req As WebRequest = WebRequest.Create(uri)
'        '
'        req.Method = "GET"
'        req.ContentType = "application/json"
'        '
'        Dim response As String
'        '
'        Using result As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
'            Dim sResponse As String = ""
'            '
'            Using srRead As New StreamReader(result.GetResponseStream())
'                sResponse = srRead.ReadToEnd()
'                response = sResponse
'                'если ответ содержит ссылку на след страницу, то будет следующий запрос
'                Dim tspositionlist As TSPositionList = JsonConvert.DeserializeObject(Of TSPositionList)(sResponse)
'                positionlist.objects.AddRange(tspositionlist.objects)
'                positionlist.meta = tspositionlist.meta
'                page = tspositionlist.meta.next
'                '
'                result.Close()
'            End Using
'        End Using
'    End While
'
'    Return positionlist
'
'End Function
'
'
''
'Public Class TSMeta
'	Public [next] As String
'	Public previous As String
'	Public pages_count As Integer
'	Public objects_count As Integer
'End Class
'
''
'Public Class TSObject
'	Public meta As TSMeta
'End Class
'
''
'Public Class TSPositionList
'	Inherits SysObject
'	Public objects As List(Of SysPosition)
'End Class
'
''
'Public Class TSPosition
'	Public id As String
'    Public description As String
'    Public office As TSOffice
'End Class
'
''
'Public Class TSOffice
'	Public id As String
'    Public description As String'
'End Class
Public Class ProcurarProdutoCtrlPedido

    Private Sub TxtProcura_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TxtProcura.KeyDown
        If e.KeyCode = Keys.F11 Then
            FecharBT_Click(sender, e)
        End If
    End Sub
    Private Sub TxtProcura_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TxtProcura.KeyPress
        Dim Tecla As Short = Asc(e.KeyChar)

        If Tecla = 13 Then
            System.Windows.Forms.SendKeys.Send("{Tab}")
            Tecla = 0
        End If

    End Sub

    Private Sub FecharBT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Atualizar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Atualizar.Click

        Dim RetornoProcura As String = ""
        Dim RetornoDesc As String = ""
        Dim RetornoValor As String = ""

        Dim I As Integer = 0

        For I = 0 To MyLista.Items.Count - 1
            If MyLista.Items(I).Selected = True Then RetornoProcura = MyLista.Items(I).Text.Substring(0)
            If MyLista.Items(I).Selected = True Then RetornoDesc = MyLista.Items(I).SubItems(1).Text.Substring(0)
        Next

        If RetornoProcura = "" Then
            MsgBox("O usu�rio deve informar um iten da lista de procura.", 64, "Valida��o de Dados")
            Me.MyLista.Focus()
            Exit Sub
        End If
        My.Forms.CompraCtrPedidoItens.Produto.Text = RetornoProcura
        My.Forms.CompraCtrPedidoItens.ProdutoDesc.Text = RetornoDesc
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub MyLista_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyLista.DoubleClick
        Atualizar_Click(sender, e)
    End Sub

    Private Sub ProcurarProdutoDetalhado_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Me.TxtProcura.Focus()
    End Sub

    Private Sub TelaProcura_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Degrade()
        Me.TxtProcura.Focus()
    End Sub

    Private Sub ProdutoEmpresa_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.TxtProcura.Focus()
    End Sub

    Private Sub MyLista_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyLista.KeyDown
        If e.KeyCode = Keys.Enter Then
            Atualizar_Click(sender, e)
        End If
    End Sub


    Private Sub TxtProcura_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtProcura.Leave
        If Me.TxtProcura.Text = "" Then
            Me.TxtProcura.Focus()
            Exit Sub
        End If

        MyLista.Items.Clear()

        Dim CampoFilter As String = ""

        If Me.ProdutoEmpresa.Checked = True Then CampoFilter = "1"
        If Me.MateriaPrima.Checked = True Then CampoFilter = "2"
        If Me.Almoxarifado.Checked = True Then CampoFilter = "3"

        Dim Cnn As New OleDb.OleDbConnection(New Conectar().ConectarBD(LocalBD & Nome_BD))
        Dim Cmd As New OleDb.OleDbCommand()
        Dim DataReader As OleDb.OleDbDataReader

        Cnn.Open()

        With Cmd
            .Connection = Cnn
            .CommandTimeout = 0
            .CommandText = "SELECT * FROM Produtos WHERE Empresa = " & CodEmpresa & " and  Inativo = false and Tipo = " & CampoFilter & " and Descri��o Like '%" & Me.TxtProcura.Text & "' & '%'"
            .CommandType = CommandType.Text
        End With

        Try
            DataReader = Cmd.ExecuteReader

            While DataReader.Read
                If Not IsDBNull(DataReader.Item("CodigoProduto")) Then
                    Dim AA As String = DataReader.Item("CodigoProduto")
                    Dim item1 As New ListViewItem(AA, 0)

                    item1.SubItems.Add(DataReader.Item("Descri��o") & "")

                    MyLista.Items.AddRange(New ListViewItem() {item1})

                End If
            End While

            DataReader.Close()

        Catch Merror As System.Exception
            MsgBox(Merror.ToString)
            If ConnectionState.Open Then
                Cnn.Close()
                Exit Sub
            End If
        End Try
        Cnn.Close()

    End Sub

    Private Sub Fechar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Fechar.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Degrade()
        Try
            Dim CCor() As String
            Dim corTemp As String

            corTemp = Cor1TelaSecundaria
            CCor = corTemp.Split(";")
            Me.Fundo.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(CCor(0), Byte), Integer), CType(CType(CCor(1), Byte), Integer), CType(CType(CCor(2), Byte), Integer), CType(CType(CCor(3), Byte), Integer))

            corTemp = Cor2TelaSecundaria
            CCor = corTemp.Split(";")
            Me.Fundo.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(CCor(0), Byte), Integer), CType(CType(CCor(1), Byte), Integer), CType(CType(CCor(2), Byte), Integer), CType(CType(CCor(3), Byte), Integer))
        Catch ex As Exception
        End Try
    End Sub

End Class
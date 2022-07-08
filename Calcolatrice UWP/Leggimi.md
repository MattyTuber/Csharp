# Semplice Calcolatrice UWP

###### V 1.0.0

Questa calcolatrice permette di eseguire, tramite l'immissione di due numeri (*num1*, *num2*) le seguenti operazioni:

- Somma:

  ``

  ```c#
  private void Button_Click_1(object sender, RoutedEventArgs e)
  {
  	float num1 = float.Parse(Num1.Text);
  
  	float num2 = float.Parse(Num2.Text);
  
  	float c = num1 + num2;
  
  	string d = c.ToString();
  
  	MessageDialog dialog = new MessageDialog(d);
  	dialog.ShowAsync();
  }
  ```

- Sottrazione:

  ``

  ```c#
  private void Button_Click_2(object sender, RoutedEventArgs e)
  {
  	float num1 = float.Parse(Num1.Text);
  
  	float num2 = float.Parse(Num2.Text);
  
  	float c = num1 - num2;
  
  	string d = c.ToString();
  
  	MessageDialog dialog = new MessageDialog(d);
  	dialog.ShowAsync();
  }
  ```

- Moltiplicazione:

  ``

  ```c#
  private void Button_Click_3(object sender, RoutedEventArgs e)
  {
  	float num1 = float.Parse(Num1.Text);
  
  	float num2 = float.Parse(Num2.Text);
  
  	float c = num1 * num2;
  
  	string d = c.ToString();
  
  	MessageDialog dialog = new MessageDialog(d);
  	dialog.ShowAsync();
  }
  ```

- Divisione:

  ``

  ```c#
  private void Button_Click_4(object sender, RoutedEventArgs e)
  {
  	float num1 = float.Parse(Num1.Text);
  
  	float num2 = float.Parse(Num2.Text);
  
  	if (num2 == 0)
  	{
  
  		MessageDialog dialog = new MessageDialog("Il secondo numero deve essere diverso da 0");
  		dialog.ShowAsync();
  
  	}
  
  	else
  	{
  
  		float c = num1 / num2;
  
  		string d = c.ToString();
  
  		MessageDialog dialog = new MessageDialog(d);
  		dialog.ShowAsync();
  
  	}
  
  }
  ```

- Elevare a potenza il primo valore:

  ``

  ```c#
  private void Button_Click(object sender, RoutedEventArgs e)
  {
  	float num1 = float.Parse(Num1.Text);
  
  	float num2 = float.Parse(Num2.Text);
  
  	double c = Math.Pow(num1, num2);
  
  	string d = c.ToString();
  
  	MessageDialog dialog = new MessageDialog(d);
  	dialog.ShowAsync();
  }
  ```

Nel momento in cui si scegli l'operazione da svolgere apparirà una finestra di dialogo con il risultato voluto, per poter procedere con una nuova operazione occorre prima chiudere la finestra tramite l'apposito tasto.



Al momento non è possibile avviare questa applicazione tramite Visual Studio 2017 e versioni inferiori alla 7,1 siccome non è possibile includere nel  `Main`  un operazione asincrona `ShowAsync`.
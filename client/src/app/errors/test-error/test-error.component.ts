import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent implements OnInit {
  baseURL = "http://localhost:5001/api/";
  validationError: string[] = [];
  constructor(private http: HttpClient) { }
  ngOnInit(): void {
  }

  get400Error() {
    console.log("you are get400Error ");

    this.http.get(this.baseURL + 'Buggy/bad-request').subscribe(res => {
      console.log(res);
    }, err => {
      console.log(err);

    });
  }
  get404Error() {
    console.log("you are get404Error ");

    this.http.get(this.baseURL + 'Buggy/not-found').subscribe(res => {
      console.log(res);
    }, err => {
      console.log(err);

    });
  }
  get500Error() {
    console.log("you are get500Error ");

    this.http.get(this.baseURL + 'Buggy/server-error').subscribe(res => {
      console.log(res);
    }, err => {
      console.log(err);

    });
  }
  get401Error() {
    console.log("you are get401Error ");
    this.http.get(this.baseURL + 'Buggy/auth').subscribe(res => {
      console.log(res);
    }, err => {
      console.log(err);

    });
  }

  get400ValidationError() {
    console.log("you are get400ValidationError ");
    this.http.post(this.baseURL + 'AccountControler/register', {}).subscribe(res => {
      console.log(res);
    }, err => {
      console.log(err);
      this.validationError = err
    });
  }

}

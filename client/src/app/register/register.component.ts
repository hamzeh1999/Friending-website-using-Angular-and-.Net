import { json } from '@angular-devkit/core';
import { JsonPipe } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Route, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  //model: any = {};
  registerForm: FormGroup;
  maxDate: Date;
  validationErrors: string[] = [];
  constructor(private router: Router, private accountService: AccountService, private toastr: ToastrService, private fb: FormBuilder) { }


  giveData() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      userName: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValue('password')]]
    });

    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    });
  }

  matchValue(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null : { isMatching: true }
    }
  }
  ngOnInit(): void {
    this.giveData();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  register() {
    // console.log(this.registerForm.value);
    this.accountService.register(this.registerForm.value).subscribe(response => {
      this.router.navigateByUrl('/members');
    }
      , error => {
        //console.log(error);
        this.validationErrors = error;
        console.log(this.validationErrors);

      //  console.log(JSON.stringify(error));
        // for(let i=0;i<error.errors.length;i++)
        // this.validationErrors[i]=error.errors[i];
        // console.log("sssss  "+this.validationErrors);
        //console.log("validation Error: "+JSON.stringify(this.validationErrors));   
        //  this.toastr.error(error.error); 
      });

  }

  cancel() {
    this.cancelRegister.emit(false);
  }




}

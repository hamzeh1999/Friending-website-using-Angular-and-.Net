import { Component } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent {
  title:string;
  message:string;
  btnOkText:string;
  btnCancelText:string;
  result:boolean;

constructor(public bsModalRef:BsModalService){

}

confirm()
{
this.result=true;
this.bsModalRef.hide();
}

decline(){
  this.result=false;
  this.bsModalRef.hide();
}

}
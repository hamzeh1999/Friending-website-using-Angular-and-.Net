import { Component, Input, ViewEncapsulation } from '@angular/core';
import { Memeber } from 'src/app/_models/Member';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'],
  encapsulation:ViewEncapsulation.None
})
export class MemberCardComponent {
  @Input() memberInput:Memeber;
}
